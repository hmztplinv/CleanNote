using MediatR;
using NoteApp.Application.Features.Auth.Dtos;
using NoteApp.Application.Interfaces.Services;
using NoteApp.Application.Wrappers;

namespace NoteApp.Application.Features.Auth.Commands;

public class LoginCommandHandler : IRequestHandler<LoginCommand, ApiResponse<TokenDto>>
{
    private readonly IAuthService _authService;

    public LoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<ApiResponse<TokenDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        return await _authService.LoginAsync(request.Username, request.Password);
    }
}