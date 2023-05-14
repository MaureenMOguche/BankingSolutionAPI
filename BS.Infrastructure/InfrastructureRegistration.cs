using BS.Application.Contracts.Email;
using BS.Application.Contracts.Logging;
using BS.Application.Models;
using BS.Infrastructure.EmailSender;
using BS.Infrastructure.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BS.Infrastructure
{
    public static class InfrastructureRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

            services.AddTransient<IBankEmailSender, BankEmailSender>();
            services.AddScoped(typeof(IBankLogger<>), typeof(LoggerAdapter<>));
            return services;
        }
    }
}
