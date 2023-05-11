using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Domain
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string? Description { get; set; }
        [Required]
        public decimal Amount { get; set; }


        //Relationships
        [Required]
        public int SenderId { get; set; }
        [ForeignKey(nameof(SenderId))]
        public BankAccount SenderAccount { get; set; }


        [Required]
        public int ReceiverId { get; set; }
        [ForeignKey(nameof(ReceiverId))]
        public BankAccount ReceiverAccount { get; set; }
    }
}
