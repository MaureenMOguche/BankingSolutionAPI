using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Application.Contracts.Logging
{
    public interface IBankLogger<T> where T : class
    {
        void LogInformation(string message, params object[] args);
        void LogWarning(string message, params object[] args);
        void LogError(string message, params object[] args);
    }
}
