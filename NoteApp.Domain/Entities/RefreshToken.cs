using NoteApp.Domain.Common;

namespace NoteApp.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public string Token { get; set; } = null!;
    public DateTime ExpiryDate { get; set; }
    public bool IsUsed { get; set; }
    public bool IsRevoked { get; set; }
    
    // Kullanıcı ilişkisi
    public Guid UserId { get; set; }
    public User? User { get; set; }
}