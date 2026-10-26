using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PrvaWebAplikacija.Auth;
using PrvaWebAplikacija.Model;
using PrvaWebAplikacija.Repository;
using PrvaWebAplikacija.Repository.Common;
using PrvaWebAplikacija.Service;
using PrvaWebAplikacija.Service.Common;
using PrvaWebAplikacija.Middleware;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) => config
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day));

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// CORS: allow the React frontend (Vite, see FrontendUrl in appsettings.json) to call this API
var frontendUrl = builder.Configuration["FrontendUrl"] ?? "http://localhost:5173";
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy => policy
        .WithOrigins(frontendUrl)
        .AllowAnyHeader()     // e.g. Authorization (the JWT token) and Content-Type
        .AllowAnyMethod());   // GET, POST, PUT, DELETE
});

// EF Core: one PraksaDbContext per HTTP request (scoped), connected to PostgreSQL
builder.Services.AddDbContext<PraksaDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnectionString")));

// Anime
builder.Services.AddScoped<IAnimeWebCatalogueRepository, AnimeWebCatalogueRepository>();
builder.Services.AddScoped<IAnimeWebCatalogueService, AnimeWebCatalogueService>();

// Manga
builder.Services.AddScoped<IMangaRepository, MangaRepository>();
builder.Services.AddScoped<IMangaService, MangaService>();

// Studio (async)
builder.Services.AddScoped<IStudioRepository, StudioRepository>();
builder.Services.AddScoped<IStudioService, StudioService>();

// User
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();


builder.Services.AddTransient<IPriceCalculator, PriceCalculator>();
builder.Services.AddSingleton<IIdGenerator, IdGenerator>();


builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddSingleton<ITokenService, TokenService>();

var jwt = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()
    ?? throw new InvalidOperationException("Missing 'Jwt' section in configuration.");

if (string.IsNullOrWhiteSpace(jwt.Key) || Encoding.UTF8.GetByteCount(jwt.Key) < 32)
    throw new InvalidOperationException(
        "Jwt:Key is missing or shorter than 32 bytes. Set it in user secrets (Manage User Secrets).");


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt.Issuer,
            ValidateAudience = true,
            ValidAudience = jwt.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            NameClaimType = "name",
            RoleClaimType = "role"
        };
    });


builder.Services.AddAuthorization();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("Frontend");   // must come before authentication
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();