using Newtonsoft.Json;

namespace CurrencyExchange.Infrastructure.Services.Models
{
    public class Error
    {
        [JsonProperty("error")]
        public string Message { get; set; }
    }
}
