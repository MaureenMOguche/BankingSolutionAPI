using BS.Application.Contracts.Persistence;
using BS.Application.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BS.Application.Features.Queries.Balance
{
    public class BalanceQueryHandler : IRequestHandler<BalanceQuery, APIResponse>
    {
        private readonly IUnitOfWork _db;

        public BalanceQueryHandler(IUnitOfWork db)
        {
            this._db = db;
        }
        public async Task<APIResponse> Handle(BalanceQuery request, CancellationToken cancellationToken)
        {
            var account = await _db.BankAccountRepo.GetOneAsync(
                x => x.BankUserId == request.userId);

            if (account == null)
            {
                return new APIResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    isSuccess = false,
                    Messages = new() { $"User does not exist" }
                };
            }

            return new APIResponse
            {
                StatusCode = HttpStatusCode.OK,
                isSuccess = true,
                Result = account.Balance
            };
        }
    }
}
