using FluentValidation;
using MediatR;
using ValidationException = CurrencyExchange.Application.Helpers.Exceptions.ValidationException;

namespace CurrencyExchange.Application.Helpers
{
    public sealed class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class, IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<object>(request);

                var failures = _validators
                                .Select(validator => validator.Validate(context))
                                .SelectMany(result => result.Errors)
                                .Where(failure => failure != null)
                                .GroupBy(failure => failure.PropertyName,
                                         failure => failure.ErrorMessage,
                                        (propertyName, errorMessages) => new
                                        {
                                            Key = propertyName,
                                            Values = errorMessages.Distinct().ToArray()
                                        })
                                .ToDictionary(error => error.Key, x => x.Values);


                if (failures.Any())
                {
                    throw new ValidationException(failures);
                }
            }

            return await next();
        }
    }
}
