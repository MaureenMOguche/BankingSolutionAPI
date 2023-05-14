using BS.Application.Contracts.Email;
using BS.Application.Contracts.Identity;
using BS.Application.Contracts.Persistence;
using BS.Application.Models;
using BS.Domain;
using BS.Persistence.EmailMessages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
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
        private readonly IBankEmailSender _bankEmailSender;

        public AuthService(UserManager<BankUser> userManager,
            SignInManager<BankUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<JwtSettings> jwtSettings,
            IUnitOfWork db,
            IBankEmailSender bankEmailSender)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
            this._jwtSettings = jwtSettings.Value;
            this._db = db;
            this._bankEmailSender = bankEmailSender;
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

            if (user.EmailConfirmed == false)
            {
                return new APIResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    isSuccess = false,
                    Messages = new() { "Please verify email" }
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
                EmailConfirmed = false
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                //Add roles if not exist
                if (!await _roleManager.RoleExistsAsync(BankUserRoles.BankManager))
                {
                    await _roleManager.CreateAsync(new IdentityRole(BankUserRoles.BankManager));
                    await _roleManager.CreateAsync(new IdentityRole(BankUserRoles.Customer));
                }

                //Addrole To user
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
                        Messages = new() { $"{createResult.Message}" }
                    };
                }



                //Send Email
                string charSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                string code = await GenerateCodeTokenAsync(6, charSet);

                var codeBytes = Encoding.UTF8.GetBytes(code);
                
                user.VerificationCode = SHA384.HashData(codeBytes);
                await _db.SaveAsync();

                //Delete verification code after 60 secs
                await DelayedTask(user);

                var emailBody = GeneralTemplate.PrefixEmailMessage(ConfirmationMail.ConfirmationEmailMessage(code));

                var emailMessage = new EmailMessage
                {
                    Subject = "Confirm Email",
                    Body = emailBody,
                    To = request.Email,
                };

                await _bankEmailSender.SendEmailAsync(emailMessage);



                //Return response

                return new APIResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    isSuccess = true,
                    Messages = new()
                    {
                        $"User {request.Email} successfully registered",
                        $"Account Number {account.AccountNumber}"
                    }
                };


            }

            var response = new APIResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                isSuccess = false,
                Messages = new()
            };
                        foreach (var err in result.Errors)
            {
                response.Messages.Add(err.Description);
            }

            return response;


        }

        public async Task<APIResponse> VerifyEmail(string code, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new APIResponse
                {
                    StatusCode = HttpStatusCode.NotFound,
                    isSuccess = false,
                    Messages = new() { "User does not exist" }
                };
            }

            if (user.VerificationCode is (byte[])null)
            {
                return new APIResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    isSuccess = false,
                    Messages = new() { "Verification code expired, generate another one" },
                };
            }


            if (code == null)
            {
                return new APIResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    isSuccess = false,
                    Messages = new() { "Code not valid" }
                };
            }

            
            var encode = Encoding.UTF8.GetBytes(code);

            var hashCode = HashAlgorithm.Create("SHA384").ComputeHash(encode);

            var hashCompare = CompareHash(hashCode, user.VerificationCode);

            if (hashCompare)
            {
                user.EmailConfirmed = true;
                await _db.SaveAsync();

                return new APIResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    isSuccess = true,
                    Messages = new() { "Successfully verified email" }
                };
            }

            return new APIResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                isSuccess = false,
                Messages = new() { "Code not valid" }
            };
        }

        private bool CompareHash(byte[] encode, byte[] hashCode)
        {
            bool bEqual = false;
            if (encode.Length == hashCode.Length)
            {
                int i = 0;
                while ((i < encode.Length) && (encode[i] == hashCode[i]))
                {
                    i += 1;
                }
                if (i == encode.Length)
                {
                    bEqual = true;
                }
            }

            return bEqual;

        }

        private async Task<string> GenerateCodeTokenAsync(int length, string charSet)
        {
            char[] result = new char[length];

            for (int i = 0; i < length; i++)
                result[i] = charSet[RandomNumberGenerator.GetInt32(charSet.Length)];

            return new string(result);
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
        private async Task DelayedTask(BankUser user)
        {
            await Task.Delay(60000);
            user.VerificationCode = (byte[])null;
            await _db.SaveAsync();
        }

        public async Task<APIResponse> GenerateNewToken(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            
            if (user == null)
            {
                return new APIResponse
                {
                    StatusCode = HttpStatusCode.NotFound,
                    isSuccess = false,
                    Messages = new() { "User does not exist" }
                };
            }

            //Send Email
            string charSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string code = await GenerateCodeTokenAsync(6, charSet);

            var codeBytes = Encoding.UTF8.GetBytes(code);

            user.VerificationCode = SHA384.HashData(codeBytes);
            await _db.SaveAsync();

            //var emailBody = $"Please use the code to verify your email {code}";
            var emailBody = GeneralTemplate.PrefixEmailMessage(ConfirmationMail.ConfirmationEmailMessage(code));

            var emailMessage = new EmailMessage
            {
                Subject = "Confirm Email",
                Body = emailBody,
                To = email,
            };

            await _bankEmailSender.SendEmailAsync(emailMessage);

            //Delete verification code after 60 secs
            await DelayedTask(user);

            return new APIResponse
            {
                StatusCode = HttpStatusCode.OK,
                isSuccess = true,
                Messages = new() { "Successfully generated new code, check email" }
            };

        }
    }
}



//Send Email


