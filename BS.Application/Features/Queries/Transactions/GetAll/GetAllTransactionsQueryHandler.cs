using AutoMapper;
using BS.Application.Contracts.Persistence;
using BS.Application.Models;
using BS.Domain;
using MediatR;
using System.Net;


namespace BS.Application.Features.Queries.Transactions.GetAll
{
    public class GetAllTransactionsQueryHandler : IRequestHandler<GetAllTransactionsQuery, APIResponse>
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;

        public GetAllTransactionsQueryHandler(IUnitOfWork db, IMapper mapper)
        {
            this._db = db;
            this._mapper = mapper;
        }
        public async Task<APIResponse> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Transaction> transactions;
            IEnumerable<TransactionsDto> transactionsDtos;

            if (request.QueryParameters.AccountNumber != null)
            {
                transactions = await _db.TransactionRepo.GetAllAsync("SenderAccount,ReceiverAccount",
                    x => x.ReceiverAccount.AccountNumber == request.QueryParameters.AccountNumber.ToString()
                    || x.SenderAccount.AccountNumber == request.QueryParameters.AccountNumber.ToString());

                if (transactions == null)
                {
                    return new APIResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        isSuccess = true,
                        Messages = new() { "There are currently no transactions" }
                    };
                }
                else
                {
                    transactionsDtos = _mapper.Map<IEnumerable<TransactionsDto>>(transactions);

                    return new APIResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        isSuccess = true,
                        Result = transactionsDtos
                    };
                }

            }

            var query = await _db.TransactionRepo.GetAllAsync("SenderAccount,ReceiverAccount");

            transactions = await _db.TransactionRepo.PaginationFilter(query, request.QueryParameters);
            
           
            if (transactions == null)
            {
                return new APIResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    isSuccess = true,
                    Messages = new() { "There are currently no transactions" }
                };
            }

            
            
            transactionsDtos = _mapper.Map<IEnumerable<TransactionsDto>>(transactions);

            return new APIResponse
            {
                StatusCode = HttpStatusCode.OK,
                isSuccess = true,
                Result = transactionsDtos
            };
        }
    }
}
