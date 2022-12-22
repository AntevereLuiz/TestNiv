using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Realiza a movimentação da conta corrente
        /// </summary>
        /// <param name="transaction">Objeto com informações da transação</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> MakeTransactionAsync(MakeTransactionCommand transaction)
        {
            try
            {
                var response = await _mediator.Send(transaction);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Obtem o saldo da conta corrente
        /// </summary>
        /// <param name="numero">Código da conta corrente</param>
        [HttpGet("{numero}")]
        [ProducesResponseType(typeof(GetBalanceQueryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetBalanceAsync(string numero)
        {
            try
            {
                var getBalanceQuery = new GetBalanceQuery() { Numero = numero };
                var response = await _mediator.Send(getBalanceQuery);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }           
        }
    }
}