using Aura.Api.Controllers;
using Aura.Api.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

// This is the main entry point of the application. It sets up the web host, configures services, and defines the middleware pipeline.
// Creo il costruttore dell'applicazione. Serve per dire che servizi voglio usare e che configurazioni.
var builder = WebApplication.CreateBuilder(args);

// Secret key for JWT signing (later, use a secure method to store this!!!)
// Chiave usate per firmare e verificare i JWT
var key = "SUPER_SECRET_KEY_123456789_SUPER_SECRET_KEY";

// Configure Services
// Sto dicendo la mia applicazione utilizzerà dei controller.
builder.Services.AddControllers();

// JWT Authentication
// Quando arriva una richiesta protetta, usa JWT per capire chi è l'utente
builder.Services.AddAuthentication(options =>
{
    // Il metodo standard sarà Bearer
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // configuro come deve essere controllato il token
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true, // se il token è scaduto
        ValidateIssuerSigningKey = true, // se è stato firmato con la chiave corretta
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)) // imposto la chiave segreta, prima l'avevo solo definita
    };
});

// Database Context
// Configuro il database. Crea il collegamento a SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        "Server=localhost\\SQLEXPRESS;Database=AuraDb;Trusted_Connection=True;TrustServerCertificate=True;"
    ));

// Dependency Injection for UserService
// Cioè sto dicendo crea una nuova istanza di UserService per ogni richiesta HTTP
builder.Services.AddScoped<UserService>(); //Vedere in futuro i vari tipi di Dipendene Injection

// OpenApi
// Servono a crare la descirzione automatica delle API, scalar utilizzerà queste informazioni per creare la grafica
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApi();

// Finalmente creo l'app
var app = builder.Build();

// Pipeline, sequenza corretta degli middleware (sequenza che ogni richiesta HTTP deve attraversare)
// In pratica sto definendo la sequenza dei middleware nella pipeline

// scalar, creo gli endpoint /scalar e /openapi
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// se arriva http, la trasforma in https
app.UseHttpsRedirection();

// Controlla chi è l'utente leggendo il JWT
app.UseAuthentication();

// Controlla se l'utente può fare l'operazione.
app.UseAuthorization();

// Collega gli URL ai controller. Per esempio GET /api/users -> UsersController.GetUsers()
app.MapControllers();

//PS: Si potrebbe introdurre un Middleweare per la gestione delle eccezioni, al posto di implemetare i try catch in ogni controller

// Avvia il server web e inizia ad ascoltare le richieste HTTP. L'app parte
app.Run();