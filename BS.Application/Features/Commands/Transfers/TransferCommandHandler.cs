using BS.Application.Constants;
using BS.Application.Contracts.Persistence;
using BS.Application.Models;
using BS.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace BS.Application.Features.Commands.Transfers
{
    public class TransferCommandHandler : IRequestHandler<TransferCommand, APIResponse>
    {
        private readonly IUnitOfWork _db;
        private readonly UserManager<BankUser> _userManager;

        public TransferCommandHandler(IUnitOfWork db, UserManager<BankUser> userManager)
        {
            this._db = db;
            this._userManager = userManager;
        }
        public async Task<APIResponse> Handle(TransferCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.userId);
            
            if (user == null)
            {
                return new APIResponse 
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    isSuccess = false,
                    Messages = new() { "Unidentified User" }
                };
            }

            var senderAccount = await _db.BankAccountRepo.GetOneAsync(x => x.BankUserId == user.Id); ;

            var receiverAccount = await _db.BankAccountRepo.GetOneAsync(
                x => x.AccountNumber == request.Request.ReceiverAccount.ToString(), "BankUser");

            if (senderAccount == null || receiverAccount == null || request.Request.Amount <= 0)
            {
                var response = new APIResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    isSuccess = false,
                    Messages = new() { "Invalid Operation" },
                };

                if (senderAccount == null)
                    response.Messages.Add($"Sender account is invalid");
                if (receiverAccount == null)
                    response.Messages.Add($"Receiver account {request.Request.ReceiverAccount} is invalid");
                if (request.Request.Amount <= 0)
                    response.Messages.Add($"Invalid amount");

                return response;
            }

            var result = await _db.BankAccountRepo.Transfer(senderAccount, 
                receiverAccount, request.Request.Amount);

            if (result == TransferDepositMessages.Success)
            {
                //Create Transactions
                var transaction = new Transaction
                {
                    Amount = request.Request.Amount,
                    SenderId = senderAccount.Id,
                    ReceiverId = receiverAccount.Id,
                    Description = request.Request.Description,
                };

                await _db.TransactionRepo.CreateAsync(transaction);
                await _db.SaveAsync();


                return new APIResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    isSuccess = true,
                    Messages = new() {
                        $"Successfully transferred {request.Request.Amount} to {request.Request.ReceiverAccount}",
                        $"logged in user: {user.FirstName} {user.LastName}"
                    }
                };
            }

            return new APIResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                isSuccess = false,
                Messages = new() {
                        $"Transfer {request.Request.Amount} to {request.Request.ReceiverAccount} not successful",
                        "Insufficient Balance"
                    }
            };


        }
    }
}
