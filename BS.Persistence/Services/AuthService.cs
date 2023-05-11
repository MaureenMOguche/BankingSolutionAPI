using BS.Application.Contracts.Identity;
using BS.Application.Contracts.Persistence;
using BS.Application.Models;
using BS.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace BS.Persistence.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<BankUser> _userManager;
        private readonly SignInManager<BankUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IUnitOfWork _db;


        public AuthService(UserManager<BankUser> userManager,
            SignInManager<BankUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<JwtSettings> jwtSettings,
            IUnitOfWork db)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
            this._jwtSettings = jwtSettings.Value;
            this._db = db;
        }
        public async Task<APIResponse> Login(LoginRequest request)
        {
            var userAccount = await _db.BankAccountRepo.GetOneAsync(
                x => x.AccountNumber == request.AccountNumber.ToString());
            
            if (userAccount == null)
            {
                return new APIResponse
                {
                    StatusCode = HttpStatusCode.NotFound,
                    isSuccess = false,
                    Messages = new() { $"Account {request.AccountNumber} Not found" }
                };
            }

            var user = await _userManager.FindByCustomParametersAsync(x => x.Id == userAccount.BankUserId);

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (!result.Succeeded)
            {
                return new APIResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    isSuccess = false,
                    Messages = new() { "Invalid Login" }
                };
            }

            JwtSecurityToken jwtSecurityToken = await GenerateTokenAsync(user);

            var authResponse = new AuthResponse
            {
                Id = user.Id,
                AccountName = $"{user.FirstName} {user.LastName}",
                AccountNumber = $"{userAccount.AccountNumber}",
                Claims = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken)
            };

            return new APIResponse
            {
                StatusCode = HttpStatusCode.OK,
                isSuccess = true,
                Messages = new()
                {
                    $"Login Successful"
                },
                Result = authResponse
            };
        }

        public async Task<APIResponse> Register(RegistrationRequest request)
        {
            //Check if user exists
            var userExists = await _userManager.FindByEmailAsync(request.Email);

            if (userExists != null)
            {
                return new APIResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    isSuccess = false,
                    Messages = new() { $"User with {request.Email} already exists" }
                };
            }

           
            //create new user
             var user = new BankUser
            {
                Email = request.Email,
                UserName = request.UserName,
                FirstName = request.FirstName,
                LastName = request.LastName,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var response = new APIResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    isSuccess = false,
                };

                foreach(var err in result.Errors)
                {
                    response.Messages.Add(err.Description);
                }

                return response;
            }

            if (!await _roleManager.RoleExistsAsync(BankUserRoles.BankManager))
            {
                await _roleManager.CreateAsync(new IdentityRole(BankUserRoles.BankManager));
                await _roleManager.CreateAsync(new IdentityRole(BankUserRoles.Customer));
            }

            await _userManager.AddToRoleAsync(user, BankUserRoles.Customer);

            //Create Account for user
            var account = new BankAccount
            {
                BankUserId = user.Id
            };
           
            var createResult = await _db.BankAccountRepo.CreateAsync(account);

            if (!createResult.isSuccess)
            {
                _userManager.DeleteAsync(user);
                return new APIResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    isSuccess = false,
                    Messages = new(){ $"{createResult.Message}" }
                };
            }

            return new APIResponse
            {
                StatusCode = HttpStatusCode.OK,
                isSuccess = true,
                Messages = new() { 
                    $"User {request.Email} successfully registered",
                    $"Account Number {account.AccountNumber}"
                }
            };

        }

        private async Task<JwtSecurityToken> GenerateTokenAsync(BankUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var userClaims = await _userManager.GetClaimsAsync(user);

            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, new Guid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName),
                new Claim(ClaimTypes.Name, user.Id),
            }
            .Union(roleClaims)
            .Union(userClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var signingCredentials = new SigningCredentials(symmetricSecurityKey,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                signingCredentials: signingCredentials,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes)
                );

            return token;
        }
    }
}
