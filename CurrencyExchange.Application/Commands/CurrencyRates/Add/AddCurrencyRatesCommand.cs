using CurrencyExchange.Application.Common.Interfaces;
using CurrencyExchange.Domain.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.Application.Commands.CurrencyRates.Add
{
    public class AddCurrencyRatesCommand : IRequest<Unit>
    {
        public string Base { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Results { get; set; }

        public class AddCurrencyRatesCommandHandler : IRequestHandler<AddCurrencyRatesCommand, Unit>
        {
            private readonly IRepository<Currencyrate> _currencyRateRepository;

            public AddCurrencyRatesCommandHandler(IRepository<Currencyrate> currencyRateRepository)
            {
                _currencyRateRepository = currencyRateRepository;
            }

            public async Task<Unit> Handle(AddCurrencyRatesCommand request, CancellationToken cancellationToken)
            {
                var isExitingCurrencyRates = _currencyRateRepository.Exists(rates => rates.Base == request.Base);

                if (isExitingCurrencyRates)
                {
                    await UpdateCurrencyRates(request, cancellationToken);
                }
                else
                {
                    var currencyRate = new Currencyrate { Base = request.Base, CreatedAt = request.CreatedAt, Results = request.Results };
                    await _currencyRateRepository.AddAsync(currencyRate);
                }

                await _currencyRateRepository.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }

            private async Task UpdateCurrencyRates(AddCurrencyRatesCommand request, CancellationToken cancellationToken)
            {
                var currencyRate = await _currencyRateRepository
                                            .GetByExpression(rates => rates.Base == request.Base)
                                            .FirstOrDefaultAsync(cancellationToken);

                currencyRate.Results = request.Results;
                _currencyRateRepository.Update(currencyRate);
            }
        }
    }
}
