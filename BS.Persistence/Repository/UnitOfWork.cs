using BS.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Persistence.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BSDbContext _db;

        public UnitOfWork(BSDbContext db)
        {
            this._db = db;
            BankAccountRepo = new BankAccountRepository(db);
            TransactionRepo = new TransactionRepository(db);
        }

        public IBankAccountRepository BankAccountRepo { get; private set; }
        public ITransactionRepository TransactionRepo { get; private set; }

        public async Task SaveAsync()
        {
            _db.SaveChangesAsync();
        }
    }
}
