using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BS.Domain
{
    public class BankUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public Byte[]? VerificationCode { get; set; }


        ////Relationships
        //[Required]
        //public int BankAccountId { get; set; }
        //[ForeignKey("BankAccountId")]
        //[ValidateNever]
        //public BankAccount BankAccount { get; set; }

    }
}
