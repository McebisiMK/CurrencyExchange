using CurrencyExchange.Application.Common.Models;

namespace CurrencyExchange.Tests.TestData
{
    public class LatestRatesTestData
    {
        public static RateModel GetApiLatestRates(string @base, DateTime createdAt)
        {
            return new RateModel
            {
                Base = @base,
                CreatedAt = createdAt,
                Rates = new Dictionary<string, decimal>
                {
                    { "AED", 3.67179M },
                    { "AFN", 71.29255M },
                    { "ALL", 95.55549M },
                    { "AMD", 394.08824M },
                    { "ANG", 1.78639M }
                }
            };
        }

        internal static CurrencyRateDTO GetExpectedLatestRates(string @base, DateTime createdAt)
        {
            return new CurrencyRateDTO
            {
                Base = @base,
                CreatedAt = createdAt,
                Results = new Dictionary<string, decimal>
                {
                    { "AED", 3.67179M },
                    { "AFN", 71.29255M },
                    { "ALL", 95.55549M },
                    { "AMD", 394.08824M },
                    { "ANG", 1.78639M }
                }
            };
        }
    }
}
