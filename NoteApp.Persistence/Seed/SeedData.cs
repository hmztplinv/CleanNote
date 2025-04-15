using NoteApp.Domain.Entities;
using NoteApp.Persistence.Contexts;

namespace NoteApp.Persistence.Seed;

public static class SeedData
{
    public static async Task InitializeAsync(NoteAppDbContext context)
    {
        if (!context.Categories.Any())
        {
            var general = new Category { Name = "General" };
            var tech = new Category { Name = "Technology" };

            var note1 = new Note
            {
                Title = "Welcome Note",
                Content = "This is a seeded note.",
                Category = general,
                IsPublished = true
            };

            var note2 = new Note
            {
                Title = "Tech Trends 2025",
                Content = "AI, ML, LLM and beyond.",
                Category = tech,
                IsPublished = true
            };

            await context.Categories.AddRangeAsync(general, tech);
            await context.Notes.AddRangeAsync(note1, note2);
            await context.SaveChangesAsync();
        }
    }
}
