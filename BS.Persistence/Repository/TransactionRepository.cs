using BS.Application.Contracts.Persistence;
using BS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Persistence.Repository
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(BSDbContext db)
            : base(db)
        {
        }
    }
}
