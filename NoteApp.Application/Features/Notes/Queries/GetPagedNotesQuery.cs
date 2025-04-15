using MediatR;
using NoteApp.Application.Features.Notes.Dtos;
using NoteApp.Application.Wrappers;

namespace NoteApp.Application.Features.Notes.Queries;

// Query to get paged notes
public class GetPagedNotesQuery : PaginationBase, IRequest<PagedResponse<List<NoteDto>>>
{
    // additional filtering parameters here if needed
}