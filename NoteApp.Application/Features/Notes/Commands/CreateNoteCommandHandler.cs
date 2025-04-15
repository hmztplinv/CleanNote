using AutoMapper;
using MediatR;
using NoteApp.Application.Interfaces.Repositories;
using NoteApp.Application.Wrappers;
using NoteApp.Domain.Entities;

namespace NoteApp.Application.Features.Notes.Commands;

// Handler to create a new note
public class CreateNoteCommandHandler : IRequestHandler<CreateNoteCommand, ApiResponse<Guid>>
{
    private readonly INoteRepository _noteRepository;
    private readonly IMapper _mapper;

    public CreateNoteCommandHandler(INoteRepository noteRepository, IMapper mapper)
    {
        _noteRepository = noteRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<Guid>> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
    {
        var newNote = new Note
        {
            Title = request.Title,
            Content = request.Content,
            CategoryId = request.CategoryId,
            IsPublished = false
        };

        await _noteRepository.AddAsync(newNote);

        return new ApiResponse<Guid>(newNote.Id);
    }
}
