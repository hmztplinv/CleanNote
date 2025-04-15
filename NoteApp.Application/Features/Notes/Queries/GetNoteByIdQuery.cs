using MediatR;
using NoteApp.Application.Features.Notes.Dtos;
using NoteApp.Application.Wrappers;

namespace NoteApp.Application.Features.Notes.Queries;

// Request to get a note by ID
public class GetNoteByIdQuery : IRequest<ApiResponse<NoteDto>>
{
    public Guid Id { get; set; }
}