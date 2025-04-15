using NoteApp.Domain.Entities;

namespace NoteApp.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByEmailAsync(string email);
    Task<List<User>> GetAllAsync();
    Task<List<string>> GetUserRolesAsync(User user);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
    Task AddToRoleAsync(User user, string roleName);
    Task AddRefreshTokenAsync(User user, RefreshToken refreshToken);
    Task<RefreshToken?> GetRefreshTokenAsync(string token);
    Task<bool> UserExistsAsync(string username);
    Task<bool> EmailExistsAsync(string email);
}