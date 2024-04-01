using CurrencyExchange.Application.Commands.CurrencyConversions.Add;
using CurrencyExchange.Application.Commands.CurrencyRates.Add;
using CurrencyExchange.Application.Queries.CurrencyConversions.Convert;
using CurrencyExchange.Application.Queries.CurrencyRates.GetLatest;
using FluentAssertions;

namespace CurrencyExchange.Tests.UnitTests
{
    [TestFixture]
    public class FluentValidatorsTests
    {
        public static readonly object[] ConvertCurrencyTestCases =
        {
            new object[] { "", "ZAR", 100M, new List<string> { "Base is required!" } },
            new object[] { "USD", "", 100M, new List<string> { "Target is required!" } },
            new object[] { "USD", "ZAR", 0M, new List<string> { "Amount is required!"} },
            new object[] { "", "", 0M, new List<string> { "Base is required!", "Target is required!", "Amount is required!" } },
        };
        
        public static readonly object[] AddCurrencyRatesTestCases =
        {
            new object[] { "", "rate results", new List<string> { "Base is required!" } },
            new object[] { "USD", "", new List<string> { "Results is required!" } },
            new object[] { "", "", new List<string> { "Base is required!", "Results is required!" } },
        };

        public static readonly object[] AddCurrencyConversionTestCases =
        {
            new object[] { "", "currency result", new List<string> { "Base is required!" } },
            new object[] { "USD", "", new List<string> { "Result is required!" } },
            new object[] { "", "", new List<string> { "Base is required!", "Result is required!" } },
        };

        [Test]
        public void Validate_Given_Empty_Base_Of_GetLatestCurrencyQuery_Should_Fail_ValidationAnd_Return_Errors()
        {
            //--------------------------------- Arrange ------------------------------------------------------
            var expectedErrors = new List<string> { "Base is required!" };

            var query = new GetLatestCurrencyRatesQuery { Base = "" };
            var validator = new GetLatestCurrencyRatesQueryValidator();

            //--------------------------------- Act     ------------------------------------------------------
            var actual = validator.Validate(query);

            //--------------------------------- Assert  ------------------------------------------------------
            actual.IsValid.Should().BeFalse();
            actual.Errors.Select(error => error.ErrorMessage).ToList().Should().BeEquivalentTo(expectedErrors);
        }

        [TestCaseSource(nameof(ConvertCurrencyTestCases))]
        public void Validate_Given_Empty_Property_Of_ConvertCurrencyQuery_Should_Fail_ValidationAnd_Return_Errors(string @base, string target, decimal amount, List<string> expectedErrors)
        {
            //--------------------------------- Arrange ------------------------------------------------------
            var query = new ConvertCurrencyQuery { BaseCurrency = @base, TargetCurrency = target, Amount = amount };
            var validator = new ConvertCurrencyQueryValidator();

            //--------------------------------- Act     ------------------------------------------------------
            var actual = validator.Validate(query);

            //--------------------------------- Assert  ------------------------------------------------------
            actual.IsValid.Should().BeFalse();
            actual.Errors.Select(error => error.ErrorMessage).ToList().Should().BeEquivalentTo(expectedErrors);
        }

        [TestCaseSource(nameof(AddCurrencyRatesTestCases))]
        public void Validate_Given_Empty_Property_Of_AddCurrencyRatesCommand_Should_Fail_ValidationAnd_Return_Errors(string @base, string results, List<string> expectedErrors)
        {
            //--------------------------------- Arrange ------------------------------------------------------
            var command = new AddCurrencyRatesCommand { Base = @base, Results = results };
            var validator = new AddCurrencyRatesCommandValidator();

            //--------------------------------- Act     ------------------------------------------------------
            var actual = validator.Validate(command);

            //--------------------------------- Assert  ------------------------------------------------------
            actual.IsValid.Should().BeFalse();
            actual.Errors.Select(error => error.ErrorMessage).ToList().Should().BeEquivalentTo(expectedErrors);
        }

        [TestCaseSource(nameof(AddCurrencyConversionTestCases))]
        public void Validate_Given_Empty_Property_Of_AddCurrencyConversionCommand_Should_Fail_ValidationAnd_Return_Errors(string @base, string result, List<string> expectedErrors)
        {
            //--------------------------------- Arrange ------------------------------------------------------
            var command = new AddCurrencyConversionCommand { Base = @base, Result = result };
            var validator = new AddCurrencyConversionCommandValidator();

            //--------------------------------- Act     ------------------------------------------------------
            var actual = validator.Validate(command);

            //--------------------------------- Assert  ------------------------------------------------------
            actual.IsValid.Should().BeFalse();
            actual.Errors.Select(error => error.ErrorMessage).ToList().Should().BeEquivalentTo(expectedErrors);
        }
    }
}
