using System;
using AspectCore.DynamicProxy;

namespace AspectCore.APM.Collector
{
    [NonAspect]
    public interface IManualPointFactory
    {
        ManualPoint Create(string measurement, DateTime? utcTimestamp = null);
    }
}