# Aura Backend API

A modern ASP.NET Core Web API built with .NET 10, featuring JWT authentication, Entity Framework Core ORM, and comprehensive API documentation.

## 📋 Prerequisites

- **.NET 10 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/10.0)
- **SQL Server** - SQL Server 2019 or later (Local or cloud instance)
- **Visual Studio 2022+** or **Visual Studio Code** (recommended: Visual Studio Community 2026)
- **Git**

## 🚀 Quick Start

### 1. Clone the Repository

```bash
git clone https://github.com/daniCalis/aura-backend.git
cd aura
```

### 2. Configure the Database Connection

Edit `Aura.Api/Program.cs` and update the SQL Server connection string:

```csharp
"Server=localhost\\SQLEXPRESS;Database=AuraDb;Trusted_Connection=True;TrustServerCertificate=True;"
```

Replace:
- `localhost\SQLEXPRESS` with your SQL Server instance name
- `AuraDb` with your desired database name

**For Azure SQL Database or remote servers:**
```csharp
"Server=your-server.database.windows.net;Database=AuraDb;User Id=sa;Password=YourPassword;Encrypt=true;TrustServerCertificate=false;"
```

### 3. Apply Database Migrations

```bash
cd Aura.Api
dotnet ef database update
```

This will create the database schema automatically.

### 4. Run the Application

```bash
dotnet run
```

The API will start on:
- **HTTPS**: `https://localhost:7000`
- **HTTP**: `http://localhost:5000`

### 5. Access API Documentation

Once running, visit:
- **Scalar UI**: `https://localhost:7000/scalar/v1`
- **OpenAPI JSON**: `https://localhost:7000/openapi/v1.json`

## 🔐 Authentication

The API uses **JWT (JSON Web Tokens)** for authentication:

1. Register a new user via `/api/users/register`
2. Login via `/api/users/login` to receive a JWT token
3. Include the token in the `Authorization` header for protected endpoints:

```bash
Authorization: Bearer YOUR_JWT_TOKEN
```

⚠️ **Important**: The JWT secret key is currently hardcoded in `Program.cs`. For production, move it to:
- Azure Key Vault
- Environment variables
- Secure configuration files (not committed to git)

## 📁 Project Structure

```
Aura.Api/
├── Controllers/          # API endpoint handlers
├── Models/              # Entity models (Database entities)
├── DTOs/                # Data Transfer Objects (Request/Response models)
├── Services/            # Business logic layer
├── Data/                # DbContext and database configuration
├── Migrations/          # EF Core database migrations
└── Program.cs           # Application entry point & middleware configuration
```

## 🛠️ Development

### Add a New Migration

```bash
dotnet ef migrations add MigrationName
```

### Update Database

```bash
dotnet ef database update
```

### Run Tests

If you have unit tests:
```bash
dotnet test
```

## 🔧 Built With

- **Framework**: ASP.NET Core (.NET 10)
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: JWT Bearer Tokens
- **API Documentation**: OpenAPI 3.0 with Scalar UI
- **Language**: C# 14.0

## 📝 Environment Variables (Optional)

Create a `.env` file or use Visual Studio's user secrets:

```bash
dotnet user-secrets set "JwtSecret" "your-secret-key"
dotnet user-secrets set "ConnectionString" "your-connection-string"
```

## 🐛 Troubleshooting

### Database Connection Issues
- Verify SQL Server is running
- Check connection string in `Program.cs`
- Ensure database user has proper permissions

### JWT Token Issues
- Verify token is sent in `Authorization: Bearer` format
- Check token expiration time
- Ensure JWT secret matches between token generation and validation

### Migration Issues
```bash
# Reset migrations (be careful - will delete data)
dotnet ef database drop
dotnet ef database update
```

## 📦 NuGet Packages

Key dependencies:
- `Microsoft.EntityFrameworkCore.SqlServer` - ORM for SQL Server
- `Microsoft.AspNetCore.Authentication.JwtBearer` - JWT authentication
- `Scalar.AspNetCore` - OpenAPI documentation UI

## 🤝 Contributing

1. Create a feature branch (`git checkout -b feature/AmazingFeature`)
2. Commit your changes (`git commit -m 'Add AmazingFeature'`)
3. Push to the branch (`git push origin feature/AmazingFeature`)
4. Open a Pull Request

## 📄 License

This project is open source. See LICENSE file for details.

## 📧 Support

For issues and questions, please open an issue on [GitHub Issues](https://github.com/daniCalis/aura-backend/issues).

---

**Happy coding! 🎉**
