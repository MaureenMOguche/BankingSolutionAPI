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


        [HttpPost("Transfer")]
        public async Task<ActionResult<APIResponse>> TransferFunds(TransferRequest request)
        {
            var user = User.Identity.Name;
            return await _mediator.Send(new TransferCommand(request, user));
        }

        [HttpPost("Deposit")]
        public async Task<ActionResult<APIResponse>> Deposit(DepositRequest request)
        {
            var user = User.Identity.Name;
            return await _mediator.Send(new DepositCommand(request, user));
        }

        [HttpGet("Balance")]
        public async Task<ActionResult<APIResponse>> Balance()
        {
            var user = User.Identity.Name;
            return await _mediator.Send(new BalanceQuery(user));
        }
    }
}
