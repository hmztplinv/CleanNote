using AutoMapper;
using MediatR;
using NoteApp.Application.Features.Notes.Dtos;
using NoteApp.Application.Interfaces.Repositories;
using NoteApp.Application.Wrappers;

namespace NoteApp.Application.Features.Notes.Queries;

// Handler for GetNoteByIdQuery
public class GetNoteByIdQueryHandler : IRequestHandler<GetNoteByIdQuery, ApiResponse<NoteDto>>
{
    private readonly INoteRepository _noteRepository;
    private readonly IMapper _mapper;

    public GetNoteByIdQueryHandler(INoteRepository noteRepository, IMapper mapper)
    {
        _noteRepository = noteRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<NoteDto>> Handle(GetNoteByIdQuery request, CancellationToken cancellationToken)
    {
        var note = await _noteRepository.GetByIdAsync(request.Id);
        
        if (note == null)
        {
            return new ApiResponse<NoteDto>($"Note with ID {request.Id} not found.");
        }

        var noteDto = _mapper.Map<NoteDto>(note);
        return new ApiResponse<NoteDto>(noteDto);
    }
}