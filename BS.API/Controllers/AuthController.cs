using Azure;
using BS.Application.Contracts.Identity;
using BS.Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            this._authService = authService;
        }


        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [HttpPost("login")]
        public async Task<ActionResult<APIResponse>> Login(LoginRequest request)
        {
            var response = await _authService.Login(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(response);
            }
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound(response);
            }
            return BadRequest(response);
        }


        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("register")]
        public async Task<ActionResult<APIResponse>> Register(RegistrationRequest request)
        {
            var response = await _authService.Register(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }


        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [HttpPost("verifyEmail")]
        public async Task<ActionResult<APIResponse>> VerifyEmail(string code, string email)
        {
            var response = await _authService.VerifyEmail(code, email);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(response);
            }
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound(response);
            }
            return BadRequest(response);
        }


        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [HttpPost("GenerateNewToken")]
        public async Task<ActionResult<APIResponse>> GenerateNewToken(string email)
        {
            var response = await _authService.GenerateNewToken(email);
            
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
    }
}
