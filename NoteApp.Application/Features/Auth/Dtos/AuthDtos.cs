namespace NoteApp.Application.Features.Auth.Dtos;

// Kullanıcı girişi için DTO
public class LoginDto
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}

// Kullanıcı kaydı için DTO
public class RegisterDto
{
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
}

// Kullanıcı bilgilerini döndürmek için DTO
public class UserDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public List<string> Roles { get; set; } = new List<string>();
}

// Token bilgilerini döndürmek için DTO
public class TokenDto
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public DateTime AccessTokenExpiration { get; set; }
    public UserDto User { get; set; } = null!;
}

// Refresh Token için DTO
public class RefreshTokenDto
{
    public string RefreshToken { get; set; } = null!;
}