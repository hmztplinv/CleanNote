using MediatR;
using NoteApp.Application.Features.Auth.Dtos;
using NoteApp.Application.Interfaces.Services;
using NoteApp.Application.Wrappers;

namespace NoteApp.Application.Features.Auth.Commands;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ApiResponse<TokenDto>>
{
    private readonly IAuthService _authService;

    public RegisterCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<ApiResponse<TokenDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (request.Password != request.ConfirmPassword)
        {
            return new ApiResponse<TokenDto>("Şifreler eşleşmiyor.");
        }

        return await _authService.RegisterAsync(request.Username, request.Email, request.Password);
    }
}