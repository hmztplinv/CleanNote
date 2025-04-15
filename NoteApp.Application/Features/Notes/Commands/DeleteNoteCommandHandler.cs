using MediatR;
using NoteApp.Application.Interfaces.Repositories;
using NoteApp.Application.Wrappers;

namespace NoteApp.Application.Features.Notes.Commands;

// Handler to delete a note
public class DeleteNoteCommandHandler : IRequestHandler<DeleteNoteCommand, ApiResponse<bool>>
{
    private readonly INoteRepository _noteRepository;

    public DeleteNoteCommandHandler(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    public async Task<ApiResponse<bool>> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
    {
        var note = await _noteRepository.GetByIdAsync(request.Id);
        
        if (note == null)
        {
            return new ApiResponse<bool>($"Note with ID {request.Id} not found.");
        }

        await _noteRepository.DeleteAsync(note);

        return new ApiResponse<bool>(true);
    }
}