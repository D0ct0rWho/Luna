using FluentValidation;
using Luna.Dtos;

namespace Luna.Validators
{
    public class CreateProductDtoValidator : AbstractValidator<CreateProductDto> // AbstractValidator<T> — базовый класс
    {
        public CreateProductDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Название продукта обязательно")
                .MinimumLength(2).WithMessage("Название должно содержать минимум 2 символа")
                .MaximumLength(100).WithMessage("Название не должно превышать 100 символов");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Цена должна быть больше нуля")
                .ScalePrecision(2, 18).WithMessage("Цена должна иметь не более двух знаков после запятой и всего до 18 цифр"); // для decimal
        }
    }
}