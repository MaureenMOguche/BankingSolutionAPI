using BS.Application.Constants;
using BS.Application.Contracts.Persistence;
using BS.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Persistence.Repository
{
    public class BankAccountRepository : GenericRepository<BankAccount>, IBankAccountRepository
    {
        private readonly BSDbContext _db;
        public BankAccountRepository(BSDbContext db)
            :base(db)
        {
            _db = db;
        }

        public async Task<string> Deposit(BankAccount bankAccount, decimal amount)
        {
            var account = await _db.BankAccounts.FirstOrDefaultAsync(x => x.Id == bankAccount.Id);
            
            if (account == null)
            {
                return TransferDepositMessages.UserNotExist;
            }
            if (amount <= 0)
                return TransferDepositMessages.ErrorInvalidAmount;

            account.Balance += amount;
            await _db.SaveChangesAsync();
            return "Success";
        }

        public async Task<string> Transfer(BankAccount senderAccount, BankAccount receiverAccount, decimal amount)
        {
            var sendAccount = await _db.BankAccounts.FirstOrDefaultAsync(x => x.Id == senderAccount.Id);
            var receiveAccount = await _db.BankAccounts.FirstOrDefaultAsync(x => x.Id ==  receiverAccount.Id);
            
            if (sendAccount == null)
                return TransferDepositMessages.UserNotExist;
            if (receiveAccount == null)
                return TransferDepositMessages.ErrorInvalidReceiverAccount;

            if (sendAccount.Balance < amount)
            {
                return TransferDepositMessages.ErrorInsufficientBalance;
            }

            sendAccount.Balance -= amount;
            receiveAccount.Balance += amount;

            _db.SaveChanges();
            return TransferDepositMessages.Success;
        }
    }
}
