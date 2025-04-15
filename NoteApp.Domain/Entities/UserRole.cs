using NoteApp.Domain.Common;

namespace NoteApp.Domain.Entities;

// Kullanıcı ve Rol arasındaki çoka-çok ilişkiyi temsil eden join entity
public class UserRole : BaseEntity
{
    public Guid UserId { get; set; }
    public User? User { get; set; }
    
    public Guid RoleId { get; set; }
    public Role? Role { get; set; }
}