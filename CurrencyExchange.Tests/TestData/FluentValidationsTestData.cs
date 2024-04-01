namespace CurrencyExchange.Tests.TestData
{
    [TestFixture]
    public class FluentValidationsTestData
    {

        public static readonly object[] AddCurrencyConversionTestCases =
        {
            new object[] { "", "currency result", new List<string> { "Base is required!" } },
            new object[] { "USD", "", new List<string> { "Result is required!" } },
            new object[] { "", "", new List<string> { "Base is required!", "Result is required!" } },
        };

        public static readonly object[] AddCurrencyRatesTestCases =
        {
            new object[] { "", "rate results", new List<string> { "Base is required!" } },
            new object[] { "USD", "", new List<string> { "Results is required!" } },
            new object[] { "", "", new List<string> { "Base is required!", "Results is required!" } },
        };
        
        public static readonly object[] ConvertCurrencyTestCases =
        {
            new object[] { "", "ZAR", 100M, new List<string> { "Base is required!" } },
            new object[] { "USD", "", 100M, new List<string> { "Target is required!" } },
            new object[] { "USD", "ZAR", 0M, new List<string> { "Amount is required!"} },
            new object[] { "", "", 0M, new List<string> { "Base is required!", "Target is required!", "Amount is required!" } },
        };
    }
}