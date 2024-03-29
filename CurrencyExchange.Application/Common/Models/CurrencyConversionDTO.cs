namespace CurrencyExchange.Application.Common.Models
{
    public class CurrencyConversionDTO
    {
        public string Base { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Dictionary<string, decimal> Result { get; set; }
    }
}
