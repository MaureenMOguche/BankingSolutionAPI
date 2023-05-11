using BS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Application.Contracts.Persistence
{
    public interface IBankAccountRepository : IGenericRepository<BankAccount>
    {
        Task<string> Transfer(BankAccount senderAccount, BankAccount receiverAccount, decimal amount);
        Task<string> Deposit(BankAccount bankAccount, decimal amount);
    }
}
