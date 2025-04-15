using NoteApp.Domain.Common;

namespace NoteApp.Domain.Entities;

public class Role : BaseEntity
{
    public string Name { get; set; } = null!;
    public string NormalizedName { get; set; } = null!;
    
    // Rol-Kullanıcı ilişkisi (çoka-çok)
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}