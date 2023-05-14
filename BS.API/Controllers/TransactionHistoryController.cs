using BS.Application.Features.Queries.Models;
using BS.Application.Features.Queries.Transactions.Download;
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


        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        [HttpGet("AllTransactions")]
        public async Task<ActionResult<APIResponse>> GetAll([FromQuery] QueryParameters queryParameters)
        {
            var user = User.Identity.Name;
            return await _mediator.Send(new GetAllTransactionsQuery(queryParameters, user));
        }

        [Authorize(Roles = BankUserRoles.BankManager)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [HttpGet("OneAccountTransactions")]
        public async Task<ActionResult<APIResponse>> GetOne()
        {
            var user = User.Identity.Name;
            var response = await _mediator.Send(new GetOneTransactionQuery(user));
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(response);
            }
            return NotFound(response);
        }


        [Authorize]
        [HttpGet("DownloadStatement")]
        public async Task<ActionResult> DownloadStatemnt()
        {
            var user = User.Identity.Name;
            var response = await _mediator.Send(new DownloadStatementQuery(user));

            return File(response.response, "application/pdf", response.fileName);
            //return Ok(response);
        }
    }
}
