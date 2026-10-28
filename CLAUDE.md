# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

Anime/manga catalogue: an ASP.NET Core Web API (.NET 10, EF Core + PostgreSQL, JWT auth) in the `PrvaWebAplikacija*` projects, plus a React 19 + Vite frontend in `Frontend/anime-catalogue`. `ConsoleApp3/` is an unrelated C# practice exercise with its own solution file. It is not part of the app.

There are no test projects.

## Commands

Backend (run from the repo root):

```bash
dotnet build PrvaWebAplikacija/PrvaWebAplikacija.slnx
dotnet run --project PrvaWebAplikacija --launch-profile http   # http://localhost:5113
```

The solution file lives inside the `PrvaWebAplikacija/` folder and references the sibling projects with `../`.

Required secrets are kept in user secrets, not in `appsettings.json`. Startup throws if `Jwt:Key` is missing or shorter than 32 bytes.

```bash
dotnet user-secrets --project PrvaWebAplikacija set "Jwt:Key" "<at least 32 bytes>"
dotnet user-secrets --project PrvaWebAplikacija set "ConnectionStrings:DefaultConnectionString" "Host=...;Database=...;Username=...;Password=..."
```

Frontend (from `Frontend/anime-catalogue`):

```bash
npm run dev     # Vite on port 5173 (strictPort)
npm run build
npm run lint    # ESLint
```

The frontend needs `Frontend/anime-catalogue/.env` with `VITE_API_URL=http://localhost:5113`. The repo's `.gitignore` ignores `*.env`, so a fresh clone has to create this file.

Ports are linked in three places. The backend's `http` profile port (`launchSettings.json`) must match `VITE_API_URL`, and the frontend origin must match `FrontendUrl` in `appsettings.json`, which sets the CORS policy.

## Architecture

### Layered backend

The backend follows a strict layered pattern, with interfaces in separate `*.Common` projects:

```
PrvaWebAplikacija (controllers, auth, middleware, DI in Program.cs)
  → Service.Common (I*Service)       ← Service (implementations)
  → Repository.Common (I*Repository) ← Repository (EF Core, PraksaDbContext)
  → Model (entities + DTOs)          → Common (exceptions, Roles, constants)
```

To add a feature, touch every layer: the model, the repository interface and implementation, the service interface and implementation, the controller, and a scoped registration in `Program.cs`.

### Database

- **Database-first, no EF migrations.** The schema source of truth is `Manga_to_Anime.sql`. It is a running script: later changes are added as `ALTER TABLE` statements near the end, mixed with practice queries. Schema changes go in that SQL file, and `PraksaDbContext.OnModelCreating` must be updated by hand to match.
- `PraksaDbContext` lives in the Repository project, but its namespace is `PrvaWebAplikacija.Model`, because it was scaffolded.
- Column and property names keep quirks from the database. For example, `Anime.ReleseDate` / `relese_date` and `Studio.NameS` / `name_s` are misspelled on purpose. Don't "fix" them without changing the database too.

### Entities vs API models

- Anime is the only resource with a separate API model. The `Anime` entity is mapped to the `AnimeWebCatalogue` DTO through the `ToModel` expression in `AnimeWebCatalogueRepository`. Names differ between the two (`Title`→`Name`), and the DTO adds `StudioName`, `MangaTitle` and `Author` from joined tables.
- Create/update anime can carry `NewManga` (a `CreateMangaDto`) instead of `MangaId`. The manga is saved in the same `SaveChangesAsync` as the anime and belongs to the anime's studio, so a failed save never leaves an orphan manga. The Add/Edit anime form's "+ Add new manga" uses this.
- The Manga and Studio repositories return EF entities directly.
- Users are exposed as `UserDto` and never include the password hash.

### Search, paging and sorting

`GET api/AnimeWebCatalogue` binds `AnimeCatalogueQuery` from the query string and returns a `PagedResult<T>`.

- Filters are applied only when sent. Text filters use Postgres `ILike`, with user text escaped through `Repository/LikePattern` so `%` and `_` match literally.
- The service clamps `PageSize` to 1–100 and `Page` to 1–100,000 (a larger page would overflow the offset).
- `Popularity` is stored as text, so sorting uses the `PopularityRank` expression (Very high > High > Medium > Low) instead of alphabetical order.

### Errors

- Services throw `NotFoundException` (mapped to 404) or `BusinessRuleException` (mapped to 400) from `PrvaWebAplikacija.Common`.
- `ExceptionHandlingMiddleware` turns these into **plain-text** response bodies. Any other exception becomes a generic 500 and is logged.
- All user-facing messages are in English, like the UI.
- Validation lives in the services: anime (title ≤ 200, genre ≤ 100, seasons 0–100, release year 1900–2100, popularity from `Common/Popularity.Options`, no duplicate title + release date), manga (`Service/MangaRules`, no duplicate title + author). Duplicate checks ignore case.
- The frontend's `request()` in `src/components/api.jsx` shows the plain-text body as the error message.

### Auth

- JWTs are issued by `Auth/TokenService` with the short claim names `sub`, `name` and `role`. `MapInboundClaims = false`, so read claims by these names, not by `ClaimTypes.*`.
- Roles are defined in `Common/Roles.cs` (`Admin`, `Customer`). Controllers use `[Authorize(Roles = Roles.Admin)]` at class level, and read endpoints opt out with `[AllowAnonymous]`.
- Registration always creates a `Customer`. Usernames must be 3–30 letters, digits, `_ . -` (checked on register only) and are unique ignoring case; login is case-insensitive too.
- An existing admin can promote a customer with `PUT api/User/{id}/make-admin` (Users page → "Make admin"); the first admin comes from the SQL seed (`borna`). Admins can't be deleted through the API (only directly in the database), and nobody can delete their own account.
- `OnTokenValidated` in `Program.cs` looks the user up on every authenticated request: a token of a deleted user, or one whose role no longer matches (e.g. just promoted), gets 401 and the frontend sends them to log in again.
- Passwords are hashed with ASP.NET Identity's `PasswordHasher<User>`.
- The frontend stores `token` and `user` in `localStorage`. `AdminRoute` hides admin pages in the UI only; the API enforces the role.

### Other services

- `IdGenerator` (a singleton) creates GUID v7 IDs for new anime. Other tables use database-side `gen_random_uuid()` defaults.

## Notes

- Serilog writes to the console and to `logs/log-<date>.txt`, relative to the working directory.
- `PrvaWebAplikacija/PrvaWebAplikacija.http` is leftover template content (`/weatherforecast`) and doesn't match the real endpoints.
