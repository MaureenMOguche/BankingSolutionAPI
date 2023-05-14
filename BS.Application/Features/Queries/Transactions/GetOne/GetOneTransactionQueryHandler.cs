using AutoMapper;
using BS.Application.Contracts.Persistence;
using BS.Application.Features.Queries.Transactions.GetAll;
using BS.Application.Models;
using BS.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Net;

namespace BS.Application.Features.Queries.Transactions.GetOne
{
    public class GetOneTransactionQueryHandler : IRequestHandler<GetOneTransactionQuery, APIResponse>
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;

        public GetOneTransactionQueryHandler(IUnitOfWork db, IMapper mapper)
        {
            this._db = db;
            this._mapper = mapper;
        }
        public async Task<APIResponse> Handle(GetOneTransactionQuery request, CancellationToken cancellationToken)
        {
            var account = await _db.BankAccountRepo.GetOneAsync(x => x.BankUserId == request.userId);

            if (account == null)
            {
                return new APIResponse
                {
                    StatusCode = HttpStatusCode.NotFound,
                    isSuccess = false,
                    Messages = new() { $"User does not exist" }
                };
            }

            var transactions = await _db.TransactionRepo.GetAllAsync("SenderAccount,ReceiverAccount",
                x => x.SenderId == account.Id || x.ReceiverId == account.Id);

            if (transactions == null)
            {
                return new APIResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    isSuccess = true,
                    Messages = new() { "There are currently no transactions" }
                };
            }


            var transactionsDtos = _mapper.Map<IEnumerable<TransactionsDto>>(transactions);

            return new APIResponse
            {
                StatusCode = HttpStatusCode.OK,
                isSuccess = true,
                Result = transactionsDtos
            };


        }
    }
}
