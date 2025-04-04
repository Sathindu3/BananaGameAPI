using BananaGameAPI.Data;
using BananaGameAPI.Models;
using BananaGameAPI.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// CORS Configuration
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // Frontend URL
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();  // Allow credentials (cookies)
        });
});

// Add services to the container.
builder.Services.AddControllers();

// ✅ Configure MySQL Database Connection
builder.Services.AddDbContext<GameDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 36)) 
    ));

// Register AuthService for Dependency Injection
builder.Services.AddScoped<AuthService>(); // Add this line to register AuthService

// Add authentication with cookie scheme
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/api/auth/login"; // Redirect to login if not authenticated
        options.LogoutPath = "/api/auth/logout"; // Redirect to logout path
        options.Cookie.HttpOnly = true;  // Make the cookie HTTP-only
        options.SlidingExpiration = true; // Enable sliding expiration
        options.ExpireTimeSpan = TimeSpan.FromMinutes(5); // Cookie expiration time
    });

// Add session services
builder.Services.AddDistributedMemoryCache(); // Use in-memory cache for session storage
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5); // Set timeout for sessions
    options.Cookie.HttpOnly = true;
});

// Add authorization
builder.Services.AddAuthorization();

// Add Scoped PasswordHasher for Player (used for hashing player passwords)
builder.Services.AddScoped<PasswordHasher<Player>>();

// Add Swagger for API Documentation (Optional)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Apply CORS policy before authentication and authorization
app.UseCors(MyAllowSpecificOrigins);

// Apply Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// Use session middleware
app.UseSession();

// Map controllers (API endpoints)
app.MapControllers();

app.Run();
