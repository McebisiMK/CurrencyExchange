namespace CurrencyExchange.Application.Common.Models
{
    public class RateModel
    {
        public string Base { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Dictionary<string, decimal> Rates { get; set; }
    }
}
