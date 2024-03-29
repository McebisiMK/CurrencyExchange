using CurrencyExchange.Application.Common.Models;
using CurrencyExchange.Application.Queries.CurrencyRates.GetLatest;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyRatesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CurrencyRatesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("latest")]
        [Produces(typeof(CurrencyRateDTO))]
        public async Task<ActionResult<CurrencyRateDTO>> GetLatestCurrencyRates(string @base)
        {
            try
            {
                var response = await _mediator.Send(new GetLatestCurrencyRatesQuery { Base = @base });

                return Ok(response);
            }
            catch (Exception exception)
            {
                exception = exception.InnerException ?? exception;
                return BadRequest(exception.Message);
            }
        }
    }
}
