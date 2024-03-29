using FluentValidation;

namespace CurrencyExchange.Application.Queries.CurrencyConversions.Convert
{
    public class ConvertCurrencyQueryValidator : AbstractValidator<ConvertCurrencyQuery>
    {
        public ConvertCurrencyQueryValidator()
        {
            RuleFor(request => request.BaseCurrency)
             .NotEmpty()
             .WithMessage("Base is required!");

            RuleFor(request => request.TargetCurrency)
             .NotEmpty()
             .WithMessage("Target is required!");

            RuleFor(request => request.Amount)
             .NotEmpty()
             .WithMessage("Amount is required!");
        }
    }
}
