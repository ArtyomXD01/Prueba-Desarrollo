using DataAccess.Interfaces;
using DataAccess.Repos;
using DataAccess.Config;
using Microsoft.AspNetCore.Mvc;
var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// IMPORTANTE: Registrar repositorios con Scoped
builder.Services.AddSingleton<DatabaseConfig>();
builder.Services.AddScoped<IRegionRepo, RegionRepo>();
builder.Services.AddScoped<IComunaRepo, ComunaRepo>();

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMVC", policy =>
    {
        policy.WithOrigins("https://localhost:7002")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowMVC");
app.UseAuthorization();
app.MapControllers();

app.Run();