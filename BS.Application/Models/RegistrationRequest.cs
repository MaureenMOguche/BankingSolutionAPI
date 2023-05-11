using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Application.Models
{
    public class RegistrationRequest
    {
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
        
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }

        [MinLength(6)]
        [Required]
        public string Password { get; set; }
    }
}
