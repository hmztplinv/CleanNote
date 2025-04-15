using AutoMapper;
using MediatR;
using NoteApp.Application.Features.Notes.Dtos;
using NoteApp.Application.Interfaces.Repositories;
using NoteApp.Application.Wrappers;

namespace NoteApp.Application.Features.Notes.Queries;

// Handler for GetAllNotesQuery
public class GetAllNotesQueryHandler : IRequestHandler<GetAllNotesQuery, ApiResponse<List<NoteDto>>>
{
    private readonly INoteRepository _noteRepository;
    private readonly IMapper _mapper;

    public GetAllNotesQueryHandler(INoteRepository noteRepository, IMapper mapper)
    {
        _noteRepository = noteRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<NoteDto>>> Handle(GetAllNotesQuery request, CancellationToken cancellationToken)
    {
        var notes = await _noteRepository.GetAllAsync();
        var mappedNotes = _mapper.Map<List<NoteDto>>(notes);
        return new ApiResponse<List<NoteDto>>(mappedNotes);
    }
}
