using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteApp.Application.Features.Auth.Commands;
using NoteApp.Application.Features.Auth.Dtos;
using NoteApp.Application.Interfaces.Services;
using NoteApp.Application.Wrappers;
using System.Security.Claims;

namespace NoteApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IAuthService _authService;

    public AuthController(IMediator mediator, IAuthService authService)
    {
        _mediator = mediator;
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<TokenDto>>> Login([FromBody] LoginDto model)
    {
        var command = new LoginCommand
        {
            Username = model.Username,
            Password = model.Password
        };

        var response = await _mediator.Send(command);

        if (!response.Success)
            return BadRequest(response);

        return Ok(response);
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<TokenDto>>> Register([FromBody] RegisterDto model)
    {
        var command = new RegisterCommand
        {
            Username = model.Username,
            Email = model.Email,
            Password = model.Password,
            ConfirmPassword = model.ConfirmPassword
        };

        var response = await _mediator.Send(command);

        if (!response.Success)
            return BadRequest(response);

        return Ok(response);
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<ApiResponse<TokenDto>>> RefreshToken([FromBody] RefreshTokenDto model)
    {
        var command = new RefreshTokenCommand
        {
            RefreshToken = model.RefreshToken
        };

        var response = await _mediator.Send(command);

        if (!response.Success)
            return BadRequest(response);

        return Ok(response);
    }

    [HttpPost("revoke-token")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<bool>>> RevokeToken([FromBody] RefreshTokenDto model)
    {
        var response = await _authService.RevokeTokenAsync(model.RefreshToken);

        if (!response.Success)
            return BadRequest(response);

        return Ok(response);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<UserDto>>> GetCurrentUser()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
        {
            return BadRequest(new ApiResponse<UserDto>("Kullanıcı kimliği alınamadı."));
        }

        var response = await _authService.GetUserByIdAsync(userGuid);

        if (!response.Success)
            return NotFound(response);

        return Ok(response);
    }
}