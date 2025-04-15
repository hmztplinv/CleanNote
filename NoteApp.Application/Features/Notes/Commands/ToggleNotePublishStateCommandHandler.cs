using MediatR;
using NoteApp.Application.Interfaces.Repositories;
using NoteApp.Application.Wrappers;

namespace NoteApp.Application.Features.Notes.Commands;

// Handler to toggle a note's publish state
public class ToggleNotePublishStateCommandHandler : IRequestHandler<ToggleNotePublishStateCommand, ApiResponse<bool>>
{
    private readonly INoteRepository _noteRepository;

    public ToggleNotePublishStateCommandHandler(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    public async Task<ApiResponse<bool>> Handle(ToggleNotePublishStateCommand request, CancellationToken cancellationToken)
    {
        var note = await _noteRepository.GetByIdAsync(request.Id);
        
        if (note == null)
        {
            return new ApiResponse<bool>($"Note with ID {request.Id} not found.");
        }

        // Toggle the publish state
        note.IsPublished = !note.IsPublished;
        note.ModifiedDate = DateTime.UtcNow;
        
        await _noteRepository.UpdateAsync(note);

        return new ApiResponse<bool>(true) 
        { 
            Message = note.IsPublished ? "Note published successfully." : "Note unpublished successfully." 
        };
    }
}