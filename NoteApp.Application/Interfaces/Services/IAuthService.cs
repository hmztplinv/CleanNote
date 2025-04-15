using NoteApp.Application.Features.Auth.Dtos;
using NoteApp.Application.Wrappers;
using NoteApp.Domain.Entities;

namespace NoteApp.Application.Interfaces.Services;

public interface IAuthService
{
    Task<ApiResponse<TokenDto>> LoginAsync(string username, string password);
    Task<ApiResponse<TokenDto>> RegisterAsync(string username, string email, string password);
    Task<ApiResponse<TokenDto>> RefreshTokenAsync(string refreshToken);
    Task<ApiResponse<bool>> RevokeTokenAsync(string refreshToken);
    Task<ApiResponse<UserDto>> GetUserByIdAsync(Guid userId);
    Task<User?> GetUserByUsernameAsync(string username);
    Task<bool> ValidateUserCredentialsAsync(string username, string password);
}