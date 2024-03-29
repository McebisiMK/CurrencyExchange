using CurrencyExchange.Application.Commands.CurrencyConversions.Add;
using CurrencyExchange.Application.Common.Interfaces;
using CurrencyExchange.Application.Common.Models;
using MediatR;
using Newtonsoft.Json;

namespace CurrencyExchange.Application.Queries.CurrencyConversions.Convert
{
    public class ConvertCurrencyQuery : IRequest<CurrencyConversionDTO>
    {
        public decimal Amount { get; set; }
        public string BaseCurrency { get; set; }
        public string TargetCurrency { get; set; }

        public class ConvertCurrencyQueryHandler : IRequestHandler<ConvertCurrencyQuery, CurrencyConversionDTO>
        {
            private readonly IMediator _mediator;
            private readonly ICurrencyExchangeService _currencyExchangeService;

            public ConvertCurrencyQueryHandler (IMediator mediator, ICurrencyExchangeService currencyExchangeService)
            {
                _mediator = mediator;
                _currencyExchangeService = currencyExchangeService;
            }

            public async Task<CurrencyConversionDTO> Handle(ConvertCurrencyQuery request, CancellationToken cancellationToken)
            {
                var externalCurrencyConversion = await _currencyExchangeService
                                                        .Convert(request.BaseCurrency, request.TargetCurrency, request.Amount);

                var currencyConversion = await SaveCurrenceConversion(externalCurrencyConversion, cancellationToken);

                 return currencyConversion;
            }

            private async Task<CurrencyConversionDTO> SaveCurrenceConversion(ConversionModel externalCurrencyConversion, CancellationToken cancellationToken)
            {
                var currencyConversion = await _mediator.Send(new AddCurrencyConversionCommand
                {
                    Base = externalCurrencyConversion.Base,
                    Amount = externalCurrencyConversion.Amount,
                    Result = JsonConvert.SerializeObject(externalCurrencyConversion.Data)
                });

                return currencyConversion;
            }
        }
    }
}
