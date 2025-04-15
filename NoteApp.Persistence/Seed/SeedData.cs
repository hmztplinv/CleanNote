using System.Security.Cryptography;
using System.Text;
using NoteApp.Domain.Entities;
using NoteApp.Persistence.Contexts;

namespace NoteApp.Persistence.Seed;

public static class SeedData
{
    public static async Task InitializeAsync(NoteAppDbContext context)
    {
        await SeedRolesAsync(context);
        await SeedUsersAsync(context);
        await SeedCategoriesAndNotesAsync(context);
    }

    private static async Task SeedRolesAsync(NoteAppDbContext context)
    {
        if (!context.Roles.Any())
        {
            var roles = new List<Role>
            {
                new Role { Name = "Admin", NormalizedName = "ADMIN" },
                new Role { Name = "User", NormalizedName = "USER" }
            };

            await context.Roles.AddRangeAsync(roles);
            await context.SaveChangesAsync();

            Console.WriteLine("Default roles added.");
        }
    }

    private static async Task SeedUsersAsync(NoteAppDbContext context)
    {
        if (!context.Users.Any())
        {
            // Admin kullanıcısı oluştur
            CreatePasswordHash("admin123", out byte[] adminPasswordHash, out byte[] adminPasswordSalt);
            
            var adminUser = new User
            {
                Username = "admin",
                Email = "admin@example.com",
                PasswordHash = Convert.ToBase64String(adminPasswordHash),
                PasswordSalt = Convert.ToBase64String(adminPasswordSalt),
                IsActive = true
            };

            // Normal kullanıcı oluştur
            CreatePasswordHash("user123", out byte[] userPasswordHash, out byte[] userPasswordSalt);
            
            var normalUser = new User
            {
                Username = "user",
                Email = "user@example.com",
                PasswordHash = Convert.ToBase64String(userPasswordHash),
                PasswordSalt = Convert.ToBase64String(userPasswordSalt),
                IsActive = true
            };

            await context.Users.AddRangeAsync(adminUser, normalUser);
            await context.SaveChangesAsync();

            // Admin kullanıcısına Admin ve User rollerini ata
            var adminRole = await context.Roles.FindAsync(context.Roles.First(r => r.Name == "Admin").Id);
            var userRole = await context.Roles.FindAsync(context.Roles.First(r => r.Name == "User").Id);

            if (adminRole != null && userRole != null)
            {
                await context.UserRoles.AddRangeAsync(
                    new UserRole { UserId = adminUser.Id, RoleId = adminRole.Id },
                    new UserRole { UserId = adminUser.Id, RoleId = userRole.Id },
                    new UserRole { UserId = normalUser.Id, RoleId = userRole.Id }
                );

                await context.SaveChangesAsync();
            }

            Console.WriteLine("Default users added.");
        }
    }

    private static async Task SeedCategoriesAndNotesAsync(NoteAppDbContext context)
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
            
            Console.WriteLine("Default categories and notes added.");
        }
    }

    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }
}