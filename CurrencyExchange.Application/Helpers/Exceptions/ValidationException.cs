using Newtonsoft.Json;

namespace CurrencyExchange.Application.Helpers.Exceptions
{
    public sealed class ValidationException : ApplicationException
    {
        public ValidationException(IReadOnlyDictionary<string, string[]> errors)
            : base("Validation error", JsonConvert.SerializeObject(errors)) => Errors = errors;

        public IReadOnlyDictionary<string, string[]> Errors { get; }
    }

    public abstract class ApplicationException : Exception
    {
        protected ApplicationException(string title, string message) : base(message) => Title = title;

        public string Title { get; }
    }
}
