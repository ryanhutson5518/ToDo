using FluentValidation;
using WebApp.Data;

namespace WebApp.Services;

public class ToDoDtoValidator : AbstractValidator<ToDoDto>
{
    public ToDoDtoValidator()
    {
        RuleFor(dto => dto.Title)
            .NotEmpty()
            .MaximumLength(Constants.ToDoTitleMaxLength);

        RuleForEach(dto => dto.ToDoItems).SetValidator(new ToDoItemValidator());
    }
}
