using FluentValidation;
using SpendingWeb.DTOs;

public class CreateCategoryDTOValidator : AbstractValidator<CreateCategoryDTO>
{
    public CreateCategoryDTOValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nama kategori wajib diisi")
            .MaximumLength(20).WithMessage("Nama kategori maksimal 20 karakter");
    }
}