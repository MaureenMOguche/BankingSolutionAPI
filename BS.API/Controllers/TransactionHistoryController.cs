using BS.Application.Features.Queries.Models;
using BS.Application.Features.Queries.Transactions.GetAll;
using BS.Application.Features.Queries.Transactions.GetOne;
using BS.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionHistoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionHistoryController(IMediator mediator)
        {
            this._mediator = mediator;
        }


        [HttpGet("AllTransactions")]
        public async Task<ActionResult<APIResponse>> GetAll([FromQuery] QueryParameters queryParameters)
        {
            var user = User.Identity.Name;
            return await _mediator.Send(new GetAllTransactionsQuery(queryParameters, user));
        }

        [Authorize]
        [HttpGet("OneAccountTransactions")]
        public async Task<ActionResult<APIResponse>> GetOne()
        {
            var user = User.Identity.Name;
            return await _mediator.Send(new GetOneTransactionQuery(user));
        }


    }
}
