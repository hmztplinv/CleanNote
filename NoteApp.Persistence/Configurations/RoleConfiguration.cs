using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoteApp.Domain.Entities;

namespace NoteApp.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.Name)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(r => r.NormalizedName)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.HasIndex(r => r.Name)
            .IsUnique();
        
        builder.HasIndex(r => r.NormalizedName)
            .IsUnique();
    }
}