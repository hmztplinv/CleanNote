using Microsoft.EntityFrameworkCore;
using NoteApp.Domain.Entities;

namespace NoteApp.Persistence.Contexts;

// EF Core DbContext implementation
public class NoteAppDbContext : DbContext
{
    public NoteAppDbContext(DbContextOptions<NoteAppDbContext> options) : base(options) { }

    public DbSet<Note> Notes => Set<Note>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Rating> Ratings => Set<Rating>();
    
    // Kimlik doğrulama için gerekli DbSet'ler
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NoteAppDbContext).Assembly);
    }
}