using MediatR;
using NoteApp.Application.Features.Notes.Dtos;
using NoteApp.Application.Wrappers;

namespace NoteApp.Application.Features.Notes.Queries;

// Request to get all notes
public class GetAllNotesQuery : IRequest<ApiResponse<List<NoteDto>>>
{
}
