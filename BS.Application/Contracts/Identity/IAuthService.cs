using BS.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Application.Contracts.Identity
{
    public interface IAuthService
    {
        Task<APIResponse> Login(LoginRequest request);
        Task<APIResponse> Register(RegistrationRequest request);
    }
}
