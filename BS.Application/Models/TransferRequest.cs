using System.ComponentModel.DataAnnotations;

namespace BS.Application.Models
{
    public class TransferRequest
    {
        //[Required]
        //public int SenderAccount { get; set; }
        [Required]
        public int ReceiverAccount { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public string? Description { get; set; }
    }
}
