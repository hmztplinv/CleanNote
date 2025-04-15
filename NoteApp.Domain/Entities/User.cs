using NoteApp.Domain.Common;

namespace NoteApp.Domain.Entities;

public class User : BaseEntity
{
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string PasswordSalt { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    
    // Kullanıcı-Rol ilişkisi (çoka-çok)
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    
    // Kullanıcı-RefreshToken ilişkisi (bire-çok)
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    
    // Note entity'si ile ilişki - bir kullanıcının birden fazla notu olabilir
    public ICollection<Note> Notes { get; set; } = new List<Note>();
}