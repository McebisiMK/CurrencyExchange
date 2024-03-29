namespace CurrencyExchange.Application.Common.Models
{
    public class CurrencyRateDTO
    {
        public string Base { get; set; }
        public DateTime CreatedAt { get; set; }
        public Dictionary<string, decimal> Results { get; set; }
    }
}
