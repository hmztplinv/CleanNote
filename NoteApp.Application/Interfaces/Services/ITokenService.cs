using System.Security.Claims;
using NoteApp.Application.Features.Auth.Dtos;
using NoteApp.Domain.Entities;

namespace NoteApp.Application.Interfaces.Services;

public interface ITokenService
{
    // JWT token oluşturma
    string GenerateJwtToken(User user, IList<string> roles);
    
    // Refresh token oluşturma
    string GenerateRefreshToken();
    
    // Token içindeki verileri çıkarma
    ClaimsPrincipal? GetPrincipalFromToken(string token);
    
    // Token ve refresh token'ı birlikte döndürme
    TokenDto GenerateTokens(User user, IList<string> roles);
    
    // Kullanıcının rollerini alma
    Task<IList<string>> GetUserRolesAsync(User user);
}