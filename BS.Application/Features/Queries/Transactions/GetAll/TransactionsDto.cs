using BS.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Application.Features.Queries.Transactions.GetAll
{
    public class TransactionsDto
    {
        public DateTime DateTime { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}
