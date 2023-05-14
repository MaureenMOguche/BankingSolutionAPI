using BS.Application.Contracts.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Infrastructure.Logging
{
    public class LoggerAdapter<T> : IBankLogger<T> where T : class
    {
        private readonly ILogger<T> _logger;

        public LoggerAdapter(ILoggerFactory loggerFactory)
        {
            this._logger = loggerFactory.CreateLogger<T>();
        }
        public void LogError(string message, params object[] args)
        {
            _logger.LogError(message, args);
        }

        public void LogInformation(string message, params object[] args)
        {
            _logger.LogInformation(message, args);
        }

        public void LogWarning(string message, params object[] args)
        {
            _logger.LogWarning(message, args);
        }
    }
}
