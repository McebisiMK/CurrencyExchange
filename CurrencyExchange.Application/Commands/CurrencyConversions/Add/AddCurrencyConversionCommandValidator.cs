using FluentValidation;

namespace CurrencyExchange.Application.Commands.CurrencyConversions.Add
{
    public class AddCurrencyConversionCommandValidator : AbstractValidator<AddCurrencyConversionCommand>
    {
        public AddCurrencyConversionCommandValidator()
        {
            RuleFor(request => request.Base)
             .NotEmpty()
             .WithMessage("Base is required!");

            RuleFor(request => request.Result)
             .NotEmpty()
             .WithMessage("Result is required!");
        }
    }
}
