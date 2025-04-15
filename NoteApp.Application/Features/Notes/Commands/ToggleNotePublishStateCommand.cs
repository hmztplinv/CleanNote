using MediatR;
using NoteApp.Application.Wrappers;

namespace NoteApp.Application.Features.Notes.Commands;

// Command to toggle note publish state
public class ToggleNotePublishStateCommand : IRequest<ApiResponse<bool>>
{
    public Guid Id { get; set; }
}