using prva_web_aplikacija.Repository;
using prva_web_aplikacija.Repository.Common;
using prva_web_aplikacija.Service;
using prva_web_aplikacija.Service.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<IAnimeWebShopRepository, AnimeWebShopRepository>();   
builder.Services.AddScoped<IAnimeWebShopService, AnimeWebShopService>();         
builder.Services.AddTransient<IPriceCalculator, PriceCalculator>();              
builder.Services.AddSingleton<IIdGenerator, IdGenerator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
