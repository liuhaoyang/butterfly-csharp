using System;
using System.Collections.Generic;
using System.Text;

namespace Butterfly.Client.Logging
{
    public interface ILoggerFactory
    {
        ILogger CreateLogger(Type type);
    }
}
