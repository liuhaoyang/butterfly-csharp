using System;
using AspectCore.DynamicProxy;

namespace AspectCore.APM.Collector
{
    [NonAspect]
    public interface ICollectorLifetime : ICollector
    {
        Action Started { get; set; }

        Action Stopped { get; set; }

        bool Start();

        void Stop();
    }
}