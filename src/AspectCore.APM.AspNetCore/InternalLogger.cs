using System;
using AspectCore.APM.Common;
using Microsoft.Extensions.Logging;

namespace AspectCore.APM.AspNetCore
{
    public class InternalLogger : IInternalLogger
    {
        private readonly ILogger<InternalLogger> _logger;

        public InternalLogger(ILogger<InternalLogger> logger)
        {
            _logger = logger;
        }

        public void LogError(string message, Exception exception)
        {
            _logger?.LogError(exception, message);
        }

        public void LogInformation(string message)
        {
            _logger?.LogInformation(message);
        }

        public void LogWarning(string message)
        {
            _logger?.LogWarning(message);
        }
    }
}