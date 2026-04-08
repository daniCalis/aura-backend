namespace Aura.Api.Models
{
    // It is a simple user model with properties for Id, Email, and PasswordHash. 
    // In a real application, you would likely have additional properties (e.g., Name, Role, etc.) and you would also want to implement proper password hashing and security practices.
    public class User
    {
        //Properties
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
