using BS.Application.Contracts.Email;
using BS.Application.Models;
using BS.Persistence.EmailMessages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilityController : ControllerBase
    {
        private readonly IBankEmailSender _emailSender;

        public UtilityController(IBankEmailSender emailSender)
        {
            this._emailSender = emailSender;
        }

        [ProducesResponseType(typeof(APIResponse), 200)]
        [HttpPost("SendMail")]
        public async Task<ActionResult<APIResponse>> SendMail(EmailRequest emailRequest)
        {
            var body = GeneralTemplate.PrefixEmailMessage(emailRequest.Body);
            EmailMessage emailMessage = new()
            {
                To = emailRequest.To,
                Subject = emailRequest.Subject,
                Body = body,
            };
            await _emailSender.SendEmailAsync(emailMessage);
            //await _emailSender.SendEmailAsync(emailMessage);
            return Ok(new APIResponse
            {
                isSuccess = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Messages = new() { $"Successfully sent mail to {emailMessage.To}" }
            });
        }
    }
}
