using FluentValidation;
using Luna.Dtos;

namespace Luna.Validators
{
    public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Название обязательно")
                .MinimumLength(2).WithMessage("Минимум 2 символа")
                .MaximumLength(100).WithMessage("Максимум 100 символов");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Цена должна быть положительной")
                .ScalePrecision(2, 18);
        }
    }
}
