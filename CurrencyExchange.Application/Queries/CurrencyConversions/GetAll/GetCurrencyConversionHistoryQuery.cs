using AutoMapper;
using CurrencyExchange.Application.Common.Interfaces;
using CurrencyExchange.Application.Common.Models;
using CurrencyExchange.Domain.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.Application.Queries.CurrencyConversions.GetAll
{
    public class GetCurrencyConversionHistoryQuery : IRequest<IList<CurrencyConversionDTO>>
    {
        public class GetCurrencyConversionHistoryQueryHandler : IRequestHandler<GetCurrencyConversionHistoryQuery, IList<CurrencyConversionDTO>>
        {
            private readonly IMapper _mapper;
            private readonly IRepository<Currencyconversion> _currencyConversionRepository;

            public GetCurrencyConversionHistoryQueryHandler(IMapper mapper, IRepository<Currencyconversion> currencyConversionRepository)
            {
                _mapper = mapper;
                _currencyConversionRepository = currencyConversionRepository;
            }

            public async Task<IList<CurrencyConversionDTO>> Handle(GetCurrencyConversionHistoryQuery request, CancellationToken cancellationToken)
            {
                var conversionHistory = await _currencyConversionRepository.GetAll().ToListAsync(cancellationToken);

                return _mapper.Map<IList<CurrencyConversionDTO>>(conversionHistory);
            }
        }
    }
}
