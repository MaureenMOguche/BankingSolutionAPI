using BS.Application.Contracts.Identity;
using BS.Application.Contracts.Persistence;
using BS.Application.Models;
using BS.Domain;
using BS.Persistence.Repository;
using BS.Persistence.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BS.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this  IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            services.AddDbContext<BSDbContext>(options => options.UseSqlServer(
                configuration.GetConnectionString("BSDbConnection")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();


            services.AddIdentity<BankUser, IdentityRole>(option =>
            {
                option.Lockout.AllowedForNewUsers = true;
                option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                option.Lockout.MaxFailedAccessAttempts = 5;
            })
                .AddEntityFrameworkStores<BSDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IAuthService, AuthService>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidAudience = configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"])),

                };
            });

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("BankManager", new AuthorizationPolicy(
                     
            //        ));
            //});

            return services;

        }
    }
}
