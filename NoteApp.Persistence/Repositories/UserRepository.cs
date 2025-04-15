using Microsoft.EntityFrameworkCore;
using NoteApp.Application.Interfaces.Repositories;
using NoteApp.Domain.Entities;
using NoteApp.Persistence.Contexts;

namespace NoteApp.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly NoteAppDbContext _context;

    public UserRepository(NoteAppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .ToListAsync();
    }

    public async Task<List<string>> GetUserRolesAsync(User user)
    {
        await _context.Entry(user)
            .Collection(u => u.UserRoles)
            .Query()
            .Include(ur => ur.Role)
            .LoadAsync();

        return user.UserRoles
            .Select(ur => ur.Role!.Name)
            .ToList();
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task AddToRoleAsync(User user, string roleName)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
        
        if (role == null)
        {
            // Eğer rol yoksa oluştur
            role = new Role
            {
                Name = roleName,
                NormalizedName = roleName.ToUpper()
            };
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
        }

        // Kullanıcının bu role sahip olup olmadığını kontrol et
        var userHasRole = await _context.UserRoles
            .AnyAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id);

        if (!userHasRole)
        {
            // Yeni UserRole kaydını DbSet'e ekle (User koleksiyonu yerine)
            var userRole = new UserRole
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                RoleId = role.Id,
                CreatedDate = DateTime.UtcNow
            };
            
            await _context.UserRoles.AddAsync(userRole);
            await _context.SaveChangesAsync();
        }
    }

    public async Task AddRefreshTokenAsync(User user, RefreshToken refreshToken)
    {
        refreshToken.UserId = user.Id;
        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
    {
        return await _context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == token);
    }

    public async Task<bool> UserExistsAsync(string username)
    {
        return await _context.Users.AnyAsync(u => u.Username == username);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }
}