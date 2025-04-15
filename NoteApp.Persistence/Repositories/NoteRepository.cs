using Microsoft.EntityFrameworkCore;
using NoteApp.Application.Interfaces.Repositories;
using NoteApp.Domain.Entities;
using NoteApp.Persistence.Contexts;

namespace NoteApp.Persistence.Repositories;

// EF Core implementation of INoteRepository
public class NoteRepository : INoteRepository
{
    private readonly NoteAppDbContext _context;

    public NoteRepository(NoteAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Note>> GetAllAsync()
    {
        return await _context.Notes
            .Include(n => n.Category)
            .Include(n => n.Ratings)
            .ToListAsync();
    }

    public async Task<Note?> GetByIdAsync(Guid id)
    {
        return await _context.Notes
            .Include(n => n.Category)
            .Include(n => n.Ratings)
            .FirstOrDefaultAsync(n => n.Id == id);
    }

    public async Task AddAsync(Note note)
    {
        await _context.Notes.AddAsync(note);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Note note)
    {
        _context.Notes.Update(note);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Note note)
    {
        _context.Notes.Remove(note);
        await _context.SaveChangesAsync();
    }
    
    public async Task<(List<Note> Notes, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
    {
        var totalCount = await _context.Notes.CountAsync();
        
        var notes = await _context.Notes
            .Include(n => n.Category)
            .Include(n => n.Ratings)
            .OrderByDescending(n => n.CreatedDate) // Most recent first
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
            
        return (notes, totalCount);
    }
}