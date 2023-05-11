using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Application.Contracts.Persistence
{
    public interface IUnitOfWork
    {
        public IBankAccountRepository BankAccountRepo { get; }
        public ITransactionRepository TransactionRepo { get; }


        Task SaveAsync();
    }
}
