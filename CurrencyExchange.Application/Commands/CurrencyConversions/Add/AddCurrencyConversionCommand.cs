using AutoMapper;
using CurrencyExchange.Application.Common.Interfaces;
using CurrencyExchange.Application.Common.Models;
using CurrencyExchange.Domain.Models.Entities;
using MediatR;

namespace CurrencyExchange.Application.Commands.CurrencyConversions.Add
{
    public class AddCurrencyConversionCommand : IRequest<CurrencyConversionDTO>
    {
        public string Base { get; set; }
        public decimal Amount { get; set; }
        public string Result { get; set; }

        public class AddCurrencyConversionCommandHandler : IRequestHandler<AddCurrencyConversionCommand, CurrencyConversionDTO>
        {
            private readonly IMapper _mapper;
            private readonly IRepository<Currencyconversion> _currencyConversionRepository;

            public AddCurrencyConversionCommandHandler(IRepository<Currencyconversion> currencyConversionRepository, IMapper mapper)
            {
                _mapper = mapper;
                _currencyConversionRepository = currencyConversionRepository;
            }

            public async Task<CurrencyConversionDTO> Handle(AddCurrencyConversionCommand request, CancellationToken cancellationToken)
            {
                var currencyConversion = new Currencyconversion
                {
                    Base = request.Base,
                    Amount = request.Amount,
                    CreatedAt = DateTime.Now,
                    Result = request.Result
                };

                await _currencyConversionRepository.AddAsync(currencyConversion);
                await _currencyConversionRepository.SaveChangesAsync(cancellationToken);

                return _mapper.Map<CurrencyConversionDTO>(currencyConversion);
            }
        }
    }
}
