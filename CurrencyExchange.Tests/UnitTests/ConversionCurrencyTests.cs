using CurrencyExchange.Application.Commands.CurrencyConversions.Add;
using CurrencyExchange.Application.Common.Interfaces;
using CurrencyExchange.Application.Common.Models;
using CurrencyExchange.Application.Mapping;
using CurrencyExchange.Application.Queries.CurrencyConversions.Convert;
using CurrencyExchange.Application.Queries.CurrencyConversions.GetAll;
using CurrencyExchange.Domain.Models.Entities;
using CurrencyExchange.Infrastructure.Exceptions;
using CurrencyExchange.Tests.Helpers;
using CurrencyExchange.Tests.TestData;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using static CurrencyExchange.Application.Queries.CurrencyConversions.Convert.ConvertCurrencyQuery;
using static CurrencyExchange.Application.Queries.CurrencyConversions.GetAll.GetCurrencyConversionHistoryQuery;

namespace CurrencyExchange.Tests.UnitTests
{
    [TestFixture]
    public class ConversionCurrencyTests
    {
        private static IMapper _mapper;

        public ConversionCurrencyTests()
        {
            if (_mapper == null)
            {
                var mappingConfiguration = new MapperConfiguration(config =>
                {
                    config.AddProfile(new CurrencyExchangeMappingProfile());
                });

                _mapper = mappingConfiguration.CreateMapper();
            }
        }

        [Test]
        public async Task GetConversionHistory_Given_The_Database_Has_No_Data_Should_Return_Empty_List_Conversion_History()
        {
            //------------------------------------- Arrange ------------------------------------------------------
            var expectedConversionHistory = new List<CurrencyConversionDTO>();
            var conversionHistoryDB = Substitute.For<DbSet<Currencyconversion>, IQueryable<Currencyconversion>, IAsyncEnumerable<Currencyconversion>>()
                                        .Initialize(new List<Currencyconversion>().AsQueryable());

            var currencyConversionRepository = Substitute.For<IRepository<Currencyconversion>>();

            var currencyConversionHistoryQueryHandler = CreateCurrencyConversionHistoryQueryHandler(currencyConversionRepository);

            currencyConversionRepository.GetAll().Returns(conversionHistoryDB);

            //------------------------------------- Act     ------------------------------------------------------
            var actual = await currencyConversionHistoryQueryHandler.Handle(new GetCurrencyConversionHistoryQuery(), CancellationToken.None);

            //------------------------------------- Assert  ------------------------------------------------------
            actual.Should().HaveCount(0);
            actual.Should().BeEquivalentTo(expectedConversionHistory);
        }

        [Test]
        public async Task GetConversionHistory_Given_Data_Is_Present_In_The_Database_Should_Return_Conversion_History()
        {
            //------------------------------------- Arrange ------------------------------------------------------
            var createdAt = DateTime.Now;
            var expectedConversionHistory = CurrencyConversionTestData.GetCurrencyConversionHistory(createdAt);
            var conversionHistoryDB = Substitute.For<DbSet<Currencyconversion>, IQueryable<Currencyconversion>, IAsyncEnumerable<Currencyconversion>>()
                                        .Initialize(CurrencyConversionTestData.GetDBCurrencyConvertionHistory(createdAt).AsQueryable());

            var currencyConversionRepository = Substitute.For<IRepository<Currencyconversion>>();

            var currencyConversionHistoryQueryHandler = CreateCurrencyConversionHistoryQueryHandler(currencyConversionRepository);

            currencyConversionRepository.GetAll().Returns(conversionHistoryDB);

            //------------------------------------- Act     ------------------------------------------------------
            var actual = await currencyConversionHistoryQueryHandler.Handle(new GetCurrencyConversionHistoryQuery(), CancellationToken.None);

            //------------------------------------- Assert  ------------------------------------------------------
            actual.Should().HaveCount(3);
            actual.Should().BeEquivalentTo(expectedConversionHistory);
        }

        [Test]
        public async Task Convert_Given_The_API_Throws_An_Exception_Should_Return_Error_With_Message()
        {
            //-------------------------------- Arrange ------------------------------------
            var expectedMessage = "Invalid/unsupported currency: ['INVALID']";
            var currencyExchangeService = Substitute.For<ICurrencyExchangeService>();

            var currencyConversionRepository = Substitute.For<IRepository<Currencyconversion>>();

            currencyExchangeService.Convert(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<decimal>()).Returns(
                Task.FromException<ConversionModel>(new ExchangeRateApiException("Invalid/unsupported currency: ['INVALID']"))
            );

            //-------------------------------- Act     ------------------------------------
            var actual = Assert.ThrowsAsync<ExchangeRateApiException>(() => currencyExchangeService.Convert("INVALID", "USD", 200M));

            //-------------------------------- Assert  ------------------------------------
            actual.Message.Should().Be(expectedMessage);
        }

        [Test]
        public async Task Convert_Given_The_API_Returns_Converted_Data_Should_Return_Save_And_Return_Data()
        {
            //-------------------------------- Arrange ------------------------------------
            var expectedConvertedData = CurrencyConversionTestData.ConvertedData("USD", "EUR", 100);
            var currencyExchangeService = Substitute.For<ICurrencyExchangeService>();

            var mediator = Substitute.For<IMediator>();
            var convertCurrencyQueryHandler = new ConvertCurrencyQueryHandler(mediator, currencyExchangeService);

            currencyExchangeService.Convert("USD", "EUR", 100).Returns(CurrencyConversionTestData.ApiConvertedData("USD", "EUR", 100));
            mediator.Send(Arg.Any<AddCurrencyConversionCommand>()).Returns(expectedConvertedData);

            //-------------------------------- Act     ------------------------------------
            var actual = await convertCurrencyQueryHandler.Handle(new ConvertCurrencyQuery { Amount = 100, BaseCurrency = "USD", TargetCurrency = "EUR" }, CancellationToken.None);

            //-------------------------------- Assert  ------------------------------------
            await currencyExchangeService.Received(1).Convert(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<decimal>());
            actual.Should().BeEquivalentTo(expectedConvertedData);
        }

        private static GetCurrencyConversionHistoryQueryHandler CreateCurrencyConversionHistoryQueryHandler(IRepository<Currencyconversion> currencyConversionRepository)
        {
            return new GetCurrencyConversionHistoryQueryHandler(_mapper, currencyConversionRepository);
        }
    }
}
