using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BS.Application.Models
{
    public class AuthResponse
    {
        public string Id { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public object Claims { get; set; }
    }
}
