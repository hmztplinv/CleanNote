using MediatR;
using NoteApp.Application.Interfaces.Repositories;
using NoteApp.Application.Wrappers;

namespace NoteApp.Application.Features.Notes.Commands;

// Handler to update an existing note
public class UpdateNoteCommandHandler : IRequestHandler<UpdateNoteCommand, ApiResponse<Guid>>
{
    private readonly INoteRepository _noteRepository;

    public UpdateNoteCommandHandler(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    public async Task<ApiResponse<Guid>> Handle(UpdateNoteCommand request, CancellationToken cancellationToken)
    {
        var note = await _noteRepository.GetByIdAsync(request.Id);
        
        if (note == null)
        {
            return new ApiResponse<Guid>($"Note with ID {request.Id} not found.");
        }

        // Update note properties
        note.Title = request.Title;
        note.Content = request.Content;
        note.CategoryId = request.CategoryId;
        note.ModifiedDate = DateTime.UtcNow;
        
        await _noteRepository.UpdateAsync(note);

        return new ApiResponse<Guid>(note.Id);
    }
}