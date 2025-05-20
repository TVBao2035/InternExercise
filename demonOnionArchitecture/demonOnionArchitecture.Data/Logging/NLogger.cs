using demonOnionArchitecture.Common.Interfaces;
using Microsoft.Extensions.Logging;
using NLog;

namespace demonOnionArchitecture.Infrastructure.Logging
{
    public class NLogger<T> : IAppLogger<T>
    {
        private readonly NLog.ILogger _logger = LogManager.GetLogger(typeof(T).FullName);
        public void LogError(string message, Exception ex = null)
        {
            _logger.Error(ex, message);
        }

        public void LogInformation(string message)
        {
            _logger.Info(message);
        }

        public void LogWarning(string message)
        {
            _logger.Warn(message);
        }
    }
}
