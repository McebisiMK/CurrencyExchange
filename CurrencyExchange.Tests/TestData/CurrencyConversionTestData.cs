using CurrencyExchange.Application.Common.Models;
using CurrencyExchange.Domain.Models.Entities;
using Newtonsoft.Json;

namespace CurrencyExchange.Tests.TestData
{
    public class CurrencyConversionTestData
    {
        public static ConversionModel ApiConvertedData(string @base, string target, decimal amount)
        {
            return new ConversionModel
            {
                Base = @base,
                Amount = amount,
                Data = new Dictionary<string, decimal>
                    {
                        { target.ToUpper(), 92.69M },
                        { "rate", 0.9269M }
                    }
            };
        }

        public static CurrencyConversionDTO ConvertedData(string @base, string target, decimal amount)
        {
            return new CurrencyConversionDTO
            {
                Base = @base,
                Amount = amount,
                Result = new Dictionary<string, decimal>
                    {
                        { target.ToUpper(), 92.69M },
                        { "rate", 0.9269M }
                    }
            };
        }

        public static List<CurrencyConversionDTO> GetCurrencyConversionHistory(DateTime createdAt)
        {
            return new List<CurrencyConversionDTO>
            {
                new CurrencyConversionDTO
                {
                    Base = "USD",
                    CreatedAt = createdAt,
                    Amount = 100,
                    Result = new Dictionary<string, decimal>
                    {
                        { "ZAR", 1896.84M },
                        { "rate", 18.96838M }
                    }
                },
                new CurrencyConversionDTO
                {
                    Base = "USD",
                    CreatedAt = createdAt.AddDays(-1),
                    Amount = 200,
                    Result = new Dictionary<string, decimal>
                    {
                        { "EUR", 185.59M },
                        { "rate", 0.92796M }
                    }
                },
                new CurrencyConversionDTO
                {
                    Base = "ZAR",
                    CreatedAt = createdAt.AddDays(-2),
                    Amount = 200,
                    Result = new Dictionary<string, decimal>
                    {
                        { "EUR", 9.8M },
                        { "rate", 0.04901M }
                    }
                }
            };
        }

        public static List<Currencyconversion> GetDBCurrencyConvertionHistory(DateTime createdAt)
        {
            return new List<Currencyconversion>
            {
                new Currencyconversion
                {
                    Base = "USD",
                    CreatedAt = createdAt,
                    Amount = 100,
                    Result = JsonConvert.SerializeObject(new Dictionary<string, decimal>
                    {
                        { "ZAR", 1896.84M },
                        { "rate", 18.96838M }
                    })
                },
                new Currencyconversion
                {
                    Base = "USD",
                    CreatedAt = createdAt.AddDays(-1),
                    Amount = 200,
                    Result = JsonConvert.SerializeObject(new Dictionary<string, decimal>
                    {
                        { "EUR", 185.59M },
                        { "rate", 0.92796M }
                    })
                },
                new Currencyconversion
                {
                    Base = "ZAR",
                    CreatedAt = createdAt.AddDays(-2),
                    Amount = 200,
                    Result = JsonConvert.SerializeObject(new Dictionary<string, decimal>
                    {
                        { "EUR", 9.8M },
                        { "rate", 0.04901M }
                    })
                }
            };
        }
    }
}
