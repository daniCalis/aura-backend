using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Aura.Api.Data;

// 🔥 This is the main entry point of the application. It sets up the web host, configures services, and defines the middleware pipeline.
var builder = WebApplication.CreateBuilder(args);

// 🔑 Secret key for JWT signing (later, use a secure method to store this)
var key = "SUPER_SECRET_KEY_123456789_SUPER_SECRET_KEY";

// 🛠️ Configure Services
builder.Services.AddControllers();

// 🔐 JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
    };
});

// 📊 Database Context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        "Server=localhost\\SQLEXPRESS;Database=AuraDb;Trusted_Connection=True;TrustServerCertificate=True;"
    ));

// Dependency Injection for UserService
builder.Services.AddScoped<UserService>();

// 📄 Swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();


var app = builder.Build();

// 🔧 Pipeline, sequenza corretta degli middleware (sequenza che ogni richiesta HTTP deve attraversare)

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 🔥 IMPORTANTISSIMO: ordine corretto
app.UseAuthentication();
app.UseAuthorization();

// Registra tutti i tuoi controllers
app.MapControllers();

// Avvia il server web e inizia ad ascoltare le richieste HTTP
app.Run();