using FluentValidation;
using SpendingWeb.DTOs;

public class CreateExpenseDTOValidator : AbstractValidator<CreateExpenseDTO>
{
    public CreateExpenseDTOValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Decription cannot be empty")
            .MaximumLength(100).WithMessage("Description must not exceed 100 characters");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Category must be selected");
    }
}