using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Options;
using NoteApp.Application.Features.Auth.Dtos;
using NoteApp.Application.Interfaces.Repositories;
using NoteApp.Application.Interfaces.Services;
using NoteApp.Application.Wrappers;
using NoteApp.Domain.Entities;
using NoteApp.Infrastructure.Settings;

namespace NoteApp.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly JwtSettings _jwtSettings;

    public AuthService(
        IUserRepository userRepository,
        ITokenService tokenService,
        IMapper mapper,
        IOptions<JwtSettings> jwtSettings)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _mapper = mapper;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<ApiResponse<TokenDto>> LoginAsync(string username, string password)
    {
        var user = await _userRepository.GetByUsernameAsync(username);

        if (user == null)
        {
            return new ApiResponse<TokenDto>("Geçersiz kullanıcı adı veya şifre");
        }

        if (!await ValidateUserCredentialsAsync(username, password))
        {
            return new ApiResponse<TokenDto>("Geçersiz kullanıcı adı veya şifre");
        }

        var userRoles = await _tokenService.GetUserRolesAsync(user);
        var tokenDto = _tokenService.GenerateTokens(user, userRoles);

        // Refresh token'ı kaydet
        var refreshToken = new RefreshToken
        {
            Token = tokenDto.RefreshToken,
            ExpiryDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
            IsUsed = false,
            IsRevoked = false,
            UserId = user.Id
        };

        await _userRepository.AddRefreshTokenAsync(user, refreshToken);

        return new ApiResponse<TokenDto>(tokenDto);
    }

    public async Task<ApiResponse<TokenDto>> RegisterAsync(string username, string email, string password)
    {
        // Kullanıcı adı ve e-posta kontrolü
        if (await _userRepository.UserExistsAsync(username))
        {
            return new ApiResponse<TokenDto>($"'{username}' kullanıcı adı zaten kullanılıyor.");
        }

        if (await _userRepository.EmailExistsAsync(email))
        {
            return new ApiResponse<TokenDto>($"'{email}' e-posta adresi zaten kullanılıyor.");
        }

        // Şifre hash'leme
        CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

        // Yeni kullanıcı oluşturma
        var user = new User
        {
            Username = username,
            Email = email,
            PasswordHash = Convert.ToBase64String(passwordHash),
            PasswordSalt = Convert.ToBase64String(passwordSalt),
            IsActive = true
        };

        await _userRepository.AddAsync(user);

        // Kullanıcıya User rolü atama
        await _userRepository.AddToRoleAsync(user, "User");

        // Token oluşturma
        var userRoles = await _tokenService.GetUserRolesAsync(user);
        var tokenDto = _tokenService.GenerateTokens(user, userRoles);

        // Refresh token kaydetme
        var refreshToken = new RefreshToken
        {
            Token = tokenDto.RefreshToken,
            ExpiryDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
            IsUsed = false,
            IsRevoked = false,
            UserId = user.Id
        };

        await _userRepository.AddRefreshTokenAsync(user, refreshToken);

        return new ApiResponse<TokenDto>(tokenDto);
    }

    public async Task<ApiResponse<TokenDto>> RefreshTokenAsync(string refreshToken)
    {
        var storedToken = await _userRepository.GetRefreshTokenAsync(refreshToken);

        if (storedToken == null)
        {
            return new ApiResponse<TokenDto>("Geçersiz refresh token.");
        }

        // Token geçerlilik kontrolü
        if (storedToken.IsUsed || storedToken.IsRevoked || storedToken.ExpiryDate < DateTime.UtcNow)
        {
            return new ApiResponse<TokenDto>("Refresh token süresi dolmuş veya geçersiz.");
        }

        // Kullanıcıyı al
        var user = await _userRepository.GetByIdAsync(storedToken.UserId);
        if (user == null)
        {
            return new ApiResponse<TokenDto>("Kullanıcı bulunamadı.");
        }

        // Mevcut token'ı kullanıldı olarak işaretle
        storedToken.IsUsed = true;
        await _userRepository.UpdateAsync(user);

        // Yeni token'lar oluştur
        var userRoles = await _tokenService.GetUserRolesAsync(user);
        var tokenDto = _tokenService.GenerateTokens(user, userRoles);

        // Yeni refresh token'ı kaydet
        var newRefreshToken = new RefreshToken
        {
            Token = tokenDto.RefreshToken,
            ExpiryDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
            IsUsed = false,
            IsRevoked = false,
            UserId = user.Id
        };

        await _userRepository.AddRefreshTokenAsync(user, newRefreshToken);

        return new ApiResponse<TokenDto>(tokenDto);
    }

    public async Task<ApiResponse<bool>> RevokeTokenAsync(string refreshToken)
    {
        var storedToken = await _userRepository.GetRefreshTokenAsync(refreshToken);

        if (storedToken == null)
        {
            return new ApiResponse<bool>("Geçersiz refresh token.");
        }

        // Token'ı iptal et
        if (!storedToken.IsUsed && !storedToken.IsRevoked)
        {
            storedToken.IsRevoked = true;
            var user = await _userRepository.GetByIdAsync(storedToken.UserId);
            if (user != null)
            {
                await _userRepository.UpdateAsync(user);
                return new ApiResponse<bool>(true);
            }
        }

        return new ApiResponse<bool>("Token zaten kullanılmış veya iptal edilmiş.");
    }

    public async Task<ApiResponse<UserDto>> GetUserByIdAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
        {
            return new ApiResponse<UserDto>($"Kullanıcı bulunamadı (ID: {userId}).");
        }

        var userDto = _mapper.Map<UserDto>(user);
        var roles = await _tokenService.GetUserRolesAsync(user);
        userDto.Roles = roles.ToList();

        return new ApiResponse<UserDto>(userDto);
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _userRepository.GetByUsernameAsync(username);
    }

    public async Task<bool> ValidateUserCredentialsAsync(string username, string password)
    {
        var user = await _userRepository.GetByUsernameAsync(username);

        if (user == null)
        {
            return false;
        }

        return VerifyPasswordHash(password, 
            Convert.FromBase64String(user.PasswordHash), 
            Convert.FromBase64String(user.PasswordSalt));
    }

    // Şifre hash'leme yardımcı metotları
    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
    {
        using var hmac = new HMACSHA512(storedSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        
        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != storedHash[i])
            {
                return false;
            }
        }

        return true;
    }
}