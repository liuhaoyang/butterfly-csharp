using System;

namespace Butterfly.Client.Logging
{
    public interface ILogger
    {
        void Info(string message);

        void Error(string message, Exception exception);
    }
}
