using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Persistence.EmailMessages
{
    public static class ConfirmationMail
    {
        public static string ConfirmationEmailMessage(string code)
        {
            string response = $"<p>Please use <span style=\"color:green;font-weight:bold\">{code}</span> to verify your email</p>";
            return response;
        }
    }
}
