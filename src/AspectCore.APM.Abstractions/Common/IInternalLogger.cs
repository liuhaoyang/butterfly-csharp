using System;
using AspectCore.DynamicProxy;

namespace AspectCore.APM.Common
{
    [NonAspect]
    public interface IInternalLogger
    {
        void LogInformation(string message);
        void LogWarning(string message);
        void LogError(string message, Exception exception);
    }
}
