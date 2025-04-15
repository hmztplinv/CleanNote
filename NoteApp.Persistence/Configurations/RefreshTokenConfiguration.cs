using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoteApp.Domain.Entities;

namespace NoteApp.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(rt => rt.Id);
        
        builder.Property(rt => rt.Token)
            .IsRequired();
        
        builder.Property(rt => rt.ExpiryDate)
            .IsRequired();
            
        builder.HasIndex(rt => rt.Token)
            .IsUnique();
    }
}