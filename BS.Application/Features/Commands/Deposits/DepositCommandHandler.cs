using BS.Application.Constants;
using BS.Application.Contracts.Persistence;
using BS.Application.Models;
using BS.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;



namespace BS.Application.Features.Commands.Deposits
{
    public class DepositCommandHandler : IRequestHandler<DepositCommand, APIResponse>
    {
        private readonly IUnitOfWork _db;
        private readonly UserManager<BankUser> _userManager;

        public DepositCommandHandler(IUnitOfWork db, UserManager<BankUser> userManager)
        {
            this._db = db;
            this._userManager = userManager;
        }
        public async Task<APIResponse> Handle(DepositCommand request, CancellationToken cancellationToken)
        {
            //var user = await _userManager.FindByIdAsync(request.userId);

            var account = await _db.BankAccountRepo.GetOneAsync(
                x => x.BankUserId == request.userId);

            if (account == null)
            {
                return new APIResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    isSuccess = false,
                    Messages = new() { "User does not exist" }
                };
            }

            var result = await _db.BankAccountRepo.Deposit(account, request.Request.Amount);

            if (result == TransferDepositMessages.ErrorInvalidAmount)
            {
                var response = new APIResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    isSuccess = false,
                    Messages = new() { "Invalid Amount" }
                };

                return response;
            }
            
            var transaction = new Transaction
            {
                Amount = request.Request.Amount,
                Description = $"Deposit",
                SenderId = account.Id,
                ReceiverId = account.Id,
                DateTime = DateTime.UtcNow,
            }; 

            await _db.TransactionRepo.CreateAsync(transaction);
            await _db.SaveAsync();
            
            return new APIResponse
            {
                StatusCode = HttpStatusCode.OK,
                isSuccess = true,
                Messages = new() {
                    $"Successfully deposited {request.Request.Amount}",
                }
            };

        }
    }
}
