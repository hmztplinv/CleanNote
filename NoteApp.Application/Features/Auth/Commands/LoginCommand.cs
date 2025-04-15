using MediatR;
using NoteApp.Application.Features.Auth.Dtos;
using NoteApp.Application.Wrappers;

namespace NoteApp.Application.Features.Auth.Commands;

public class LoginCommand : IRequest<ApiResponse<TokenDto>>
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}