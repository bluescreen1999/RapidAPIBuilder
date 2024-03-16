using FluentValidation;
using RapidAPIBuilder.Models.Dtos.Tags;

namespace RapidAPIBuilder.Models.Validators.Tags;

public class CreateTagRequestValidator : AbstractValidator<CreateTagRequest>
{
    public CreateTagRequestValidator()
    {
        RuleFor(_ => _.Title)
            .NotEmpty().WithMessage("The {PropertyName} field must not be empty.")
            .NotNull().WithMessage("The {PropertyName} field must not be null.")
            .MaximumLength(150).WithMessage("The {PropertyName} field must not exceed 150 characters.");
    }
}

