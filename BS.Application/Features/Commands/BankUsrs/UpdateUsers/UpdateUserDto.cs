using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Application.Features.Commands.BankUsrs.UpdateUsers
{
    public class UpdateUserDto
    {
        public DateTime DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
    }
}
