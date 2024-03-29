using CurrencyExchange.Application.Common.Models;

namespace CurrencyExchange.Application.Common.Interfaces
{
    public interface ICurrencyExchangeService
    {
        Task<RateModel> GetLatestRates(string baseCurrency);
        Task<ConversionModel> Convert(string baseCurrency, string targetCurrency, decimal amount);
    }
}
