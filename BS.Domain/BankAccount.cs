using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BS.Domain
{
    public class BankAccount
    {
        private static int _initial = 1100220000;

        public BankAccount()
        {
            AccountNumber = _initial.ToString();
            _initial++;
            Balance = 0;
        }

        [Key]
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }


        //Relationships

        //[Required]
        //public int AccountTypeId { get; set; }
        //[ValidateNever]
        //[ForeignKey(nameof(AccountTypeId))]
        //public AccountType AccountType { get; set; }

        //Relationships
        [Required]
        public string BankUserId { get; set; }
        [ForeignKey(nameof(BankUserId))]
        [ValidateNever]
        public BankUser BankUser { get; set; }
    }
}
