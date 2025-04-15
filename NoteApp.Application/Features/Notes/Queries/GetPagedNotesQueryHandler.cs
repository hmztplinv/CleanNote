using AutoMapper;
using MediatR;
using NoteApp.Application.Features.Notes.Dtos;
using NoteApp.Application.Interfaces.Repositories;
using NoteApp.Application.Wrappers;

namespace NoteApp.Application.Features.Notes.Queries;

// Handler for GetPagedNotesQuery
public class GetPagedNotesQueryHandler : IRequestHandler<GetPagedNotesQuery, PagedResponse<List<NoteDto>>>
{
    private readonly INoteRepository _noteRepository;
    private readonly IMapper _mapper;

    public GetPagedNotesQueryHandler(INoteRepository noteRepository, IMapper mapper)
    {
        _noteRepository = noteRepository;
        _mapper = mapper;
    }

    public async Task<PagedResponse<List<NoteDto>>> Handle(GetPagedNotesQuery request, CancellationToken cancellationToken)
    {
        var (notes, totalCount) = await _noteRepository.GetPagedAsync(request.PageNumber, request.PageSize);
        
        var notesDtoList = _mapper.Map<List<NoteDto>>(notes);
        
        return new PagedResponse<List<NoteDto>>(
            notesDtoList, 
            request.PageNumber, 
            request.PageSize, 
            totalCount
        );
    }
}