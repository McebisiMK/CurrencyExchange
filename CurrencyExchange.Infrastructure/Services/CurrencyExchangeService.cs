using CurrencyExchange.Application.Common.Models;
using CurrencyExchange.Application.Common.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;
using Error = CurrencyExchange.Infrastructure.Services.Models.Error;
using CurrencyExchange.Infrastructure.Exceptions;

namespace CurrencyExchange.Infrastructure.Services
{
    public class CurrencyExchangeService : ICurrencyExchangeService
    {
        private readonly string apiKey;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public CurrencyExchangeService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            apiKey = _configuration.GetValue<string>("CurrencyExchangeAPIOptions:ApiKey");
        }

        public async Task<ConversionModel> Convert(string baseCurrency, string targetCurrency, decimal amount)
        {
            var relativeUrl = $"/convert?api_key={apiKey}&from={baseCurrency}&to={targetCurrency}&amount={amount}";
            var response = await _httpClient.GetAsync(relativeUrl);

            var currencyConversion = new ConversionModel();

            if (response.IsSuccessStatusCode)
            {
                var stringContent = await response.Content.ReadAsStringAsync();
                var conversionJObject = JObject.Parse(stringContent);
                var result = JsonConvert.SerializeObject(conversionJObject.SelectToken("result"));

                currencyConversion = JsonConvert.DeserializeObject<ConversionModel>(stringContent);
                currencyConversion.Data = JsonConvert.DeserializeObject<Dictionary<string, decimal>>(result);
            }
            else
            {
                var error = await HandleErrors(response);
                throw new ExchangeRateApiException($"Exchange Rates API threw exception. Error: {error.Message}");
            }

            return currencyConversion;
        }

        public async Task<RateModel> GetLatestRates(string baseCurrency)
        {
            var relativeUrl = $"/fetch-all?api_key={apiKey}&from={baseCurrency}";
            var response = await _httpClient.GetAsync(relativeUrl);

            var currencyRates = new RateModel();

            if (response.IsSuccessStatusCode)
            {
                var stringContent = await response.Content.ReadAsStringAsync();
                var ratesJObject = JObject.Parse(stringContent);
                var results = JsonConvert.SerializeObject(ratesJObject.SelectToken("results"));

                currencyRates = JsonConvert.DeserializeObject<RateModel>(stringContent);
                currencyRates.Rates = JsonConvert.DeserializeObject<Dictionary<string, decimal>>(results);
            }
            else
            {
                var error = await HandleErrors(response);
                throw new ExchangeRateApiException($"Exchange Rates API threw exception. Error: {error.Message}");
            }

            return currencyRates;
        }

        private async Task<Error> HandleErrors(HttpResponseMessage response)
        {
            return JsonConvert.DeserializeObject<Error>(await response.Content.ReadAsStringAsync());
        }
    }
}
