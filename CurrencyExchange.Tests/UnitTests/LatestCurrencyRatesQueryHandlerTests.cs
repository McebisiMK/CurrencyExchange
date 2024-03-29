using CurrencyExchange.Application.Common.Interfaces;
using CurrencyExchange.Application.Common.Models;
using CurrencyExchange.Application.Mapping;
using CurrencyExchange.Application.Queries.CurrencyRates.GetLatest;
using CurrencyExchange.Infrastructure.Exceptions;
using CurrencyExchange.Tests.TestData;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using static CurrencyExchange.Application.Queries.CurrencyRates.GetLatest.GetLatestCurrencyRatesQuery;

namespace CurrencyExchange.Tests.UnitTests
{
    [TestFixture]
    public class LatestCurrencyRatesQueryHandlerTests
    {
        private static IMapper _mapper;
        private IConfiguration _configuration;

        public LatestCurrencyRatesQueryHandlerTests()
        {
            if (_mapper == null)
            {
                var mappingConfiguration = new MapperConfiguration(config =>
                {
                    config.AddProfile(new CurrencyExchangeMappingProfile());
                });

                _mapper = mappingConfiguration.CreateMapper();
            }

            _configuration = ConfigurationsTestData.CreateInMemoryConfigurations();
        }

        [Test]
        public async Task GetLatestCurrencyRates_Given_No_Data_Found_In_Cache_For_Given_Key_And_The_API_Throws_An_Exception_Should_Return_Error_With_Message()
        {
            //-------------------------------- Arrange ------------------------------------
            var expectedMessage = "Invalid/unsupported currency: ['INVALID']";
            var cacheService = Substitute.For<ICacheService>();
            var currencyExchangeService = Substitute.For<ICurrencyExchangeService>();

            var latestCurrencyRatesQueryHandler = CreateLatestCurrencyRatesQueryHandler(cacheService, currencyExchangeService);

            cacheService.GetCachedData<RateModel>(Arg.Any<string>()).Returns(Task.FromResult(default(RateModel)));
            currencyExchangeService.GetLatestRates("INVALID").Returns(
                Task.FromException<RateModel>(new ExchangeRateApiException("Invalid/unsupported currency: ['INVALID']"))
            );

            //-------------------------------- Act     ------------------------------------
            var actual = Assert.ThrowsAsync<ExchangeRateApiException>(() => currencyExchangeService.GetLatestRates("INVALID"));

            //-------------------------------- Assert  ------------------------------------
            actual.Message.Should().Be(expectedMessage);
        }

        [Test]
        public async Task GetLatestCurrencyRates_Given_No_Data_Found_In_Cache_For_Given_Key_Should_Call_The_API_And_Return_Data()
        {
            //-------------------------------- Arrange ------------------------------------
            var createdAt = DateTime.Now;
            var expectedCurrencyRates = LatestRatesTestData.GetExpectedLatestRates("USD", createdAt);
            var latestCurrencyRates = LatestRatesTestData.GetApiLatestRates("USD", createdAt);

            var cacheService = Substitute.For<ICacheService>();
            var currencyExchangeService = Substitute.For<ICurrencyExchangeService>();

            var latestCurrencyRatesQueryHandler = CreateLatestCurrencyRatesQueryHandler(cacheService, currencyExchangeService);

            cacheService.GetCachedData<RateModel>("USD").Returns(Task.FromResult(default(RateModel)));
            currencyExchangeService.GetLatestRates("USD").Returns(Task.FromResult(latestCurrencyRates));

            //-------------------------------- Act     ------------------------------------
            var actual = await latestCurrencyRatesQueryHandler.Handle(new GetLatestCurrencyRatesQuery { Base = "USD" }, CancellationToken.None);

            //-------------------------------- Assert  ------------------------------------
            await cacheService.Received(1).GetCachedData<RateModel>(Arg.Any<string>());
            await currencyExchangeService.Received(1).GetLatestRates(Arg.Any<string>());
            await cacheService.Received(1).SetCacheData(Arg.Any<string>(), Arg.Any<RateModel>(), Arg.Any<TimeSpan>());
            actual.Should().BeEquivalentTo(expectedCurrencyRates);
        }

        [Test]
        public async Task GetLatestCurrencyRates_Given_Data_Was_Present_In_Cache_For_Given_Key_Should_Return_Cache_Data()
        {
            //-------------------------------- Arrange ------------------------------------
            var createdAt = DateTime.Now;
            var expectedCurrencyRates = LatestRatesTestData.GetExpectedLatestRates("USD", createdAt);
            var latestCurrencyRates = LatestRatesTestData.GetApiLatestRates("USD", createdAt);

            var cacheService = Substitute.For<ICacheService>();
            var currencyExchangeService = Substitute.For<ICurrencyExchangeService>();

            var latestCurrencyRatesQueryHandler = CreateLatestCurrencyRatesQueryHandler(cacheService, currencyExchangeService);

            cacheService.GetCachedData<RateModel>("USD").Returns(Task.FromResult(latestCurrencyRates));

            //-------------------------------- Act     ------------------------------------
            var actual = await latestCurrencyRatesQueryHandler.Handle(new GetLatestCurrencyRatesQuery { Base = "USD" }, CancellationToken.None);

            //-------------------------------- Assert  ------------------------------------
            await cacheService.Received(1).GetCachedData<RateModel>(Arg.Any<string>());
            await currencyExchangeService.Received(0).GetLatestRates(Arg.Any<string>());
            await cacheService.Received(0).SetCacheData(Arg.Any<string>(), Arg.Any<RateModel>(), Arg.Any<TimeSpan>());
            actual.Should().BeEquivalentTo(expectedCurrencyRates);
        }

        private GetLatestCurrencyRatesQueryHandler CreateLatestCurrencyRatesQueryHandler(ICacheService cacheService, ICurrencyExchangeService currencyExchangeService)
        {
            return new GetLatestCurrencyRatesQueryHandler(_mapper, cacheService, _configuration, currencyExchangeService);
        }
    }
}
