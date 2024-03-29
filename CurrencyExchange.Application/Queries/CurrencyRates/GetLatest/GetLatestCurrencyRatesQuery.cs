using AutoMapper;
using CurrencyExchange.Application.Common.Interfaces;
using CurrencyExchange.Application.Common.Models;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace CurrencyExchange.Application.Queries.CurrencyRates.GetLatest
{
    public class GetLatestCurrencyRatesQuery : IRequest<CurrencyRateDTO>
    {
        public string Base { get; set; }

        public class GetLatestCurrencyRatesQueryHandler : IRequestHandler<GetLatestCurrencyRatesQuery, CurrencyRateDTO>
        {
            private readonly IMapper _mapper;
            private readonly ICacheService _cacheService;
            private readonly IConfiguration _configuration;
            private readonly ICurrencyExchangeService _currencyExchangeService;

            public GetLatestCurrencyRatesQueryHandler
            (
                IMapper mapper,
                ICacheService cacheService,
                IConfiguration configuration,
                ICurrencyExchangeService currencyExchangeService
            )
            {
                _mapper = mapper;
                _cacheService = cacheService;
                _configuration = configuration;
                _currencyExchangeService = currencyExchangeService;
            }

            public async Task<CurrencyRateDTO> Handle(GetLatestCurrencyRatesQuery request, CancellationToken cancellationToken)
            {
                var cacheExpirationSeconds = _configuration.GetValue<int>("CacheExpirationSeconds");

                var latestCurrencyRates = await _cacheService.GetCachedData<RateModel>(request.Base);

                if (latestCurrencyRates == null)
                {
                    latestCurrencyRates = await _currencyExchangeService.GetLatestRates(request.Base);
                    await _cacheService.SetCacheData(request.Base, latestCurrencyRates, TimeSpan.FromSeconds(cacheExpirationSeconds));
                }


                return _mapper.Map<CurrencyRateDTO>(latestCurrencyRates);
            }
        }
    }
}
