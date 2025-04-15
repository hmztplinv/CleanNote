using NoteApp.Domain.Common;

namespace NoteApp.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; } = null!;
    public List<Note> Notes { get; set; } = new();
}
