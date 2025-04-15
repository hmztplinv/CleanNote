using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoteApp.Domain.Entities;

namespace NoteApp.Persistence.Configurations;

// Note entity configuration
public class NoteConfiguration : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.HasKey(n => n.Id);
        builder.Property(n => n.Title).IsRequired().HasMaxLength(200);
        builder.Property(n => n.Content).IsRequired();
        builder.HasOne(n => n.Category)
               .WithMany(c => c.Notes)
               .HasForeignKey(n => n.CategoryId);
    }
}
