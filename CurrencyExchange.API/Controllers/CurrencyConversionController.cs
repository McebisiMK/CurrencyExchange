using CurrencyExchange.Application.Common.Models;
using CurrencyExchange.Application.Queries.CurrencyConversions.Convert;
using CurrencyExchange.Application.Queries.CurrencyConversions.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyConversionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CurrencyConversionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("convert")]
        [Produces(typeof(CurrencyConversionDTO))]
        public async Task<ActionResult<CurrencyConversionDTO>> Convert(string @base, string target, decimal amount)
        {
            try
            {
                var response = await _mediator.Send(new ConvertCurrencyQuery { BaseCurrency = @base, TargetCurrency = target, Amount = amount });

                return Ok(response);
            }
            catch (Exception exception)
            {
                exception = exception.InnerException ?? exception;
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("history")]
        [Produces(typeof(IList<CurrencyConversionDTO>))]
        public async Task<ActionResult<IList<CurrencyConversionDTO>>> GetConversionHistory()
        {
            try
            {
                var response = await _mediator.Send(new GetCurrencyConversionHistoryQuery());

                return Ok(response);
            }
            catch (Exception exception)
            {
                exception = exception.InnerException ?? exception;
                return BadRequest(exception?.Message);
            } 
        }
    }
}
