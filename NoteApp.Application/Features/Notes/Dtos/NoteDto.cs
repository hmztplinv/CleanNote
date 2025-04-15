namespace NoteApp.Application.Features.Notes.Dtos;

public class NoteDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public bool IsPublished { get; set; }
    public string? CategoryName { get; set; }
    public double? AverageRating { get; set; }
}
