using FluentValidation;

namespace NoteApp.Application.Features.Notes.Commands;

// Validation rules for creating a note
public class CreateNoteCommandValidator : AbstractValidator<CreateNoteCommand>
{
    public CreateNoteCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required.");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Category is required.");
    }
}
