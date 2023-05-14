using BS.Application.Features.Commands.Deposits;
using BS.Application.Features.Commands.Transfers;
using BS.Application.Features.Queries.Balance;
using BS.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace BS.API.Controllers
{
    [Authorize(Roles = BankUserRoles.Customer)]
    [Route("api/[controller]")]
    [ApiController]
    public class DepositTransferController : ControllerBase
    {
        private readonly IMediator _mediator;
        

        public DepositTransferController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("Transfer")]
        public async Task<ActionResult<APIResponse>> TransferFunds(TransferRequest request)
        {
            var user = User.Identity.Name;
            var response = await _mediator.Send(new TransferCommand(request, user));
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }


        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("Deposit")]
        public async Task<ActionResult<APIResponse>> Deposit(DepositRequest request)
        {
            var user = User.Identity.Name;
            var response = await _mediator.Send(new DepositCommand(request, user));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }


        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        [HttpGet("Balance")]
        public async Task<ActionResult<APIResponse>> Balance()
        {
            var user = User.Identity.Name;
            return Ok(await _mediator.Send(new BalanceQuery(user)));
        }
    }
}
