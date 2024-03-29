using Microsoft.Extensions.Configuration;

namespace CurrencyExchange.Tests.TestData
{
    public class ConfigurationsTestData
    {
        public static IConfiguration CreateInMemoryConfigurations()
        {
            var testConfigurations = new List<KeyValuePair<string, string?>>
            {
                new KeyValuePair<string, string?>("CacheExpirationSeconds", "900"),
            }.AsEnumerable();

            var configuration = new ConfigurationBuilder()
                                    .AddInMemoryCollection(testConfigurations)
                                    .Build();

            return configuration;
        }
    }
}
