namespace CurrencyExchange.Application.Common.Models
{
    public class ConversionModel
    {
        public string Base { get; set; }
        public decimal Amount { get; set; }
        public Dictionary<string, decimal> Data { get; set; }
    }
}
