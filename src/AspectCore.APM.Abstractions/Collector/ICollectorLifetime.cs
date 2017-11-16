using System;
using System.Collections.Generic;
using System.Text;
using AspectCore.DynamicProxy;

namespace AspectCore.APM.Collector
{
    [NonAspect]
    public interface ICollectorLifetime : ICollector
    {
        bool Start();

        void Stop();
    }
}