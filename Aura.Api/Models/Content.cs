namespace Aura.Api.Models
{
    // Unified content model for blog posts, expenses, or any data representation
    // Can be extended with metadata as needed
    public class Content
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Title { get; set; }
        public string Type { get; set; } = "BlogPost"; // "BlogPost", "Expense", etc.
        public string? Description { get; set; }
        
        // 🔗 Google Integration
        public string? GoogleDocsId { get; set; }   // Link to Google Docs
        public string? GoogleSheetsId { get; set; } // Link to Google Sheets
        
        // Tracking
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        
        // User reference (for future multi-user support)
        public Guid UserId { get; set; }
        public User? User { get; set; } // Navigation property
    }
}
