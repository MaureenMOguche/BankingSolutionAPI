using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BS.Application.Models;

namespace BS.Application.Contracts.Email
{
    public interface IBankEmailSender
    {
        Task<bool> SendEmailAsync(EmailMessage emailMessage);
    }
}
