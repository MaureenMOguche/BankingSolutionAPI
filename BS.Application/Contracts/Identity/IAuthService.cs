using BS.Application.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Application.Contracts.Identity
{
    public interface IAuthService
    {
        Task<APIResponse> GenerateNewToken(string email);
        Task<APIResponse> Login(LoginRequest request);
        Task<APIResponse> Register(RegistrationRequest request);
        Task<APIResponse> VerifyEmail(string code, string email);
    }
}
