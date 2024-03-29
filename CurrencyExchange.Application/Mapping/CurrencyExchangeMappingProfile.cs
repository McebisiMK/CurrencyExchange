using AutoMapper;
using CurrencyExchange.Application.Common.Models;
using CurrencyExchange.Domain.Models.Entities;
using Newtonsoft.Json;

namespace CurrencyExchange.Application.Mapping
{
    public class CurrencyExchangeMappingProfile : Profile
    {
        public CurrencyExchangeMappingProfile()
        {
            CreateMap<Currencyconversion, CurrencyConversionDTO>()
             .ForMember(dest => dest.Result, options =>
                            options.MapFrom(src => JsonConvert.DeserializeObject<Dictionary<string, decimal>>(src.Result))
              ).ReverseMap();

            CreateMap<RateModel, CurrencyRateDTO>()
             .ForMember(dest => dest.Results, options => options.MapFrom(src => src.Rates));
        }
    }
}
