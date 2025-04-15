using MediatR;
using NoteApp.Application.Wrappers;

namespace NoteApp.Application.Features.Notes.Commands;

// Command to delete a note
public class DeleteNoteCommand : IRequest<ApiResponse<bool>>
{
    public Guid Id { get; set; }
}