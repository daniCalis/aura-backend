using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Aura.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// 🔑 Chiave segreta (per ora hardcoded)
var key = "SUPER_SECRET_KEY_123456789_SUPER_SECRET_KEY";

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

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        "Server=localhost\\SQLEXPRESS;Database=AuraDb;Trusted_Connection=True;TrustServerCertificate=True;"
    ));

// 📦 Dependency Injection
builder.Services.AddScoped<UserService>();

// 📄 Swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();


var app = builder.Build();

// 🔧 Pipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 🔥 IMPORTANTISSIMO: ordine corretto
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();