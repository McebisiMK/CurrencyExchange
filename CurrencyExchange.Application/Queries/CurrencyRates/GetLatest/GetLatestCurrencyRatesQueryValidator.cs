using FluentValidation;

namespace CurrencyExchange.Application.Queries.CurrencyRates.GetLatest
{
    public class GetLatestCurrencyRatesQueryValidator : AbstractValidator<GetLatestCurrencyRatesQuery>
    {
        public GetLatestCurrencyRatesQueryValidator()
        {
            RuleFor(request => request.Base)
             .NotEmpty()
             .WithMessage("Base is required!");
        }
    }
}
