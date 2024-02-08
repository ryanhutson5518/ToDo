using FluentValidation;
using WebApp.Data;

namespace WebApp.Services;

public class ToDoItemValidator : AbstractValidator<ToDoItemDto>
{
    public ToDoItemValidator()
    {
        RuleFor(dto => dto.Description)
            .NotEmpty()
            .MaximumLength(Constants.ToDoItemDescriptionMaxLength);
    }
}
