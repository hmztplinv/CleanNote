using NoteApp.Domain.Entities;

namespace NoteApp.Application.Interfaces.Repositories;

public interface INoteRepository
{
    Task<List<Note>> GetAllAsync();
    Task<Note?> GetByIdAsync(Guid id);
    Task AddAsync(Note note);
    Task UpdateAsync(Note note);
    Task DeleteAsync(Note note);

    Task<(List<Note> Notes, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize);
}
