using BS.Application.Features.Commands.BankUsrs.UpdateUsers;
using BS.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankUserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BankUserController(IMediator mediator, IWebHostEnvironment webHostEnvironment)
        {
            this._mediator = mediator;
            this._webHostEnvironment = webHostEnvironment;
        }
        // GET: api/<BankUserController>
        //[HttpGet]
        //public Task<ActionResult<APIResponse>> GetUsers()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<BankUserController>/5
        //[HttpGet("{id}")]
        //public Task<ActionResult<APIResponse>> GetOneUser(int id)
        //{
        //    return "value";
        //}

        [Authorize]
        [HttpPut("UpdateUser"), ValidateAntiForgeryToken]
        public async Task<ActionResult<APIResponse>> UpdateUser(UpdateUserDto updateUser)
        {
            var user = User.Identity.Name;
            var response = await _mediator.Send(new UpdateUserCommand(updateUser, user));
            return response;
            
        }

        // POST api/<BankUserController>
        //[HttpPost]
        //public Task<ActionResult<APIResponse>> CreateAdminUser([FromBody] string value)
        //{
        //}
        //// DELETE api/<BankUserController>/5
        //[HttpDelete("{id}")]
        //public Task<ActionResult<APIResponse>> DeleteUser(int id)
        //{
        //}
    }
}
