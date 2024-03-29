using FluentValidation;

namespace CurrencyExchange.Application.Commands.CurrencyRates.Add
{
    public class AddCurrencyRatesCommandValidator : AbstractValidator<AddCurrencyRatesCommand>
    {
        public AddCurrencyRatesCommandValidator()
        {
            RuleFor(request => request.Base)
             .NotEmpty()
             .WithMessage("Base is required!");

            RuleFor(request => request.Results)
             .NotEmpty()
             .WithMessage("Results is required!");
        }
    }
}
