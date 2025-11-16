using mewo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using mewo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
// In Program.cs

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // This is the line that fixes it
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();

// Swagger with JWT support
builder.Services.AddSwagerJwtAuth();

// Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection")));

// Add JWT authentication
builder.Services.AddJwtAuth(builder.Configuration);

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ⚠️ Correct order: Authentication first, then Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
