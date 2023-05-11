using BS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Application.Contracts.Persistence
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
    }
}
