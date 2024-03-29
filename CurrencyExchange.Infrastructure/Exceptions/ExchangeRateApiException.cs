namespace CurrencyExchange.Infrastructure.Exceptions
{
    public class ExchangeRateApiException : Exception
    {
        public ExchangeRateApiException(string message) : base(message) { }
    }
}
