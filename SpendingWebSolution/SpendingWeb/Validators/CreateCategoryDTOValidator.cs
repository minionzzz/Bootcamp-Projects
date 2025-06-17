using FluentValidation;
using SpendingWeb.DTOs;

public class CreateCategoryDTOValidator : AbstractValidator<CreateCategoryDTO>
{
    public CreateCategoryDTOValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category name is required")
            .MaximumLength(20).WithMessage("Category name must not exceed 20 characters");
    }
}