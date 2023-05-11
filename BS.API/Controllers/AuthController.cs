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


        [HttpPost("login")]
        public async Task<ActionResult<APIResponse>> Login(LoginRequest request)
        {
            return await _authService.Login(request);
        }

        [HttpPost("register")]
        public async Task<ActionResult<APIResponse>> Register(RegistrationRequest request)
        {
            return await _authService.Register(request);
        }
    }
}
