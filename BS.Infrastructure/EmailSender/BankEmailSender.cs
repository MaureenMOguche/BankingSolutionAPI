using BS.Application.Contracts.Email;
using BS.Application.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace BS.Infrastructure.EmailSender
{
    public class BankEmailSender : IBankEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public BankEmailSender(IOptions<EmailSettings> emailSettings)
        {
            this._emailSettings = emailSettings.Value;
        }
        public Task<bool> SendEmailAsync(EmailMessage emailMessage)
        {
            var emailToSend = new MimeMessage();
            emailToSend.From.Add(MailboxAddress.Parse(_emailSettings.Email));
            emailToSend.To.Add(MailboxAddress.Parse(emailMessage.To));
            emailToSend.Subject = emailMessage.Subject;
            emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html){ Text = emailMessage.Body };
            //emailToSend.Attachments.
            //send email
            using (var smtp = new SmtpClient()) 
            {
                smtp.Connect(_emailSettings.SmtpServer, _emailSettings.Port, true);
                smtp.Authenticate(_emailSettings.Email, _emailSettings.Password);
                smtp.Send(emailToSend);
                smtp.Disconnect(true);
            }

            return Task.FromResult(true);
        }
    }
}
