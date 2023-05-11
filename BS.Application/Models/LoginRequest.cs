using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Application.Models
{
    public class LoginRequest
    {
        public int AccountNumber { get; set; }
        public string Password { get; set; }
    }
}
