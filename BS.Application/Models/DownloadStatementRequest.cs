using BS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Application.Models
{
    public class DownloadStatementRequest
    {
        public string AccountNumer { get; set; }
        public string AccountName { get; set; }
        public IEnumerable<Transaction> Transactions { get; set; }
    }
}
