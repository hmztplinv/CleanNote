using MediatR;
using NoteApp.Application.Wrappers;

namespace NoteApp.Application.Features.Notes.Commands;

// Command to create a new note
public class CreateNoteCommand : IRequest<ApiResponse<Guid>>
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public Guid CategoryId { get; set; }
}
