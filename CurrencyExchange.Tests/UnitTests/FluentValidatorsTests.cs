using CurrencyExchange.Application.Commands.CurrencyConversions.Add;
using CurrencyExchange.Application.Commands.CurrencyRates.Add;
using CurrencyExchange.Application.Queries.CurrencyConversions.Convert;
using CurrencyExchange.Application.Queries.CurrencyRates.GetLatest;
using CurrencyExchange.Tests.TestData;
using FluentAssertions;

namespace CurrencyExchange.Tests.UnitTests
{
    [TestFixture]
    public class FluentValidatorsTests : FluentValidationsTestData
    {
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
