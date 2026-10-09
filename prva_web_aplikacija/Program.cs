using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using prva_web_aplikacija.Model;
using prva_web_aplikacija.Repository;
using prva_web_aplikacija.Repository.Common;
using prva_web_aplikacija.Service;
using prva_web_aplikacija.Service.Common;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// EF Core: one PraksaDbContext per HTTP request (scoped), connected to PostgreSQL
builder.Services.AddDbContext<PraksaDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnectionString")));

// Anime
builder.Services.AddScoped<IAnimeWebShopRepository, AnimeWebShopRepository>();
builder.Services.AddScoped<IAnimeWebShopService, AnimeWebShopService>();

// Manga
builder.Services.AddScoped<IMangaRepository, MangaRepository>();
builder.Services.AddScoped<IMangaService, MangaService>();

// Studio (async)
builder.Services.AddScoped<IStudioRepository, StudioRepository>();
builder.Services.AddScoped<IStudioService, StudioService>();

// Helpers: transient and singleton lifetimes
builder.Services.AddTransient<IPriceCalculator, PriceCalculator>();
builder.Services.AddSingleton<IIdGenerator, IdGenerator>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
