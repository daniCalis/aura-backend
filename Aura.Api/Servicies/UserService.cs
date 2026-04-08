using Aura.Api.Data;
using Aura.Api.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

public class UserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    //Services
    public async Task RegisterAsync(string email, string password)
    {
        //Defensive validation
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required");

        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password is required");

        email = NormalizeEmail(email);

        //Check if user already exists
        if (_context.Users.Any(u => u.Email == email))
        {
            throw new InvalidOperationException("User already exists");
        }

        //Hash password
        var passwordHash = HashPassword(password);

        //Create user
        var user = new User
        {
            Email = email,
            PasswordHash = passwordHash
        };

        //Save
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<string> LoginAsync(string email, string password)
    {
        //Defensive validation
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required");

        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password is required");

        email = NormalizeEmail(email);

        var user = _context.Users.FirstOrDefault(u => u.Email == email);

        //Check user
        if (user == null)
            throw new UnauthorizedAccessException("Invalid credentials");

        var passwordHash = HashPassword(password);

        //Check PasswordHash
        if (user.PasswordHash != passwordHash)
            throw new UnauthorizedAccessException("Invalid credentials");

        return GenerateJwtToken(user);
    }

    //Helpers
    private string GenerateJwtToken(User user)
    {
        // Create a symmetric security key using a secret string.
        // Symmetric means that it's used to sign the the token and verify its authenticity
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SUPER_SECRET_KEY_123456789_SUPER_SECRET_KEY"));
        // Define the signing credentials:
        // - which key to use
        // - which algorithm (HMAC SHA256 in this case)
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Define the claims (data stored inside the token)
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        // Create the JWT token object
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        // Convert the token object into a compact string format:
        // HEADER.PAYLOAD.SIGNATURE
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }

    private string NormalizeEmail(string email)
    {
        return email.Trim().ToLowerInvariant();
    }
}