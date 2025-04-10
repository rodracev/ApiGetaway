using ApiGetaway.Data;
using ApiGetaway.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Ocelot
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);

// EF Core
builder.Services.AddDbContext<ApiKeyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication("ApiKeyScheme")
    .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthHandler>("ApiKeyScheme", null);

//builder.Services.AddOcelot();

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
// API Key Auth

var app = builder.Build();

// Middleware
app.UseAuthentication();
app.UseAuthorization();

//await app.UseOcelot();
app.UseOcelot().Wait();
app.Run();