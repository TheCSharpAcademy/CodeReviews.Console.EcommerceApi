using ECommerceApp.RyanW84.Data.DTO;
using FluentValidation;

namespace ECommerceApp.RyanW84.Validators;

public class ApiRequestDtoValidator<T> : AbstractValidator<ApiRequestDto<T>> where T : class
{
    public ApiRequestDtoValidator(IValidator<T> payloadValidator)
    {
        RuleFor(x => x.Payload)
            .NotNull().WithMessage("Payload is required.");

        When(x => x.Payload is not null, () =>
        {
            RuleFor(x => x.Payload!)
                .SetValidator(payloadValidator);
        });
    }
}
