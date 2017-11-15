using System;
using System.Collections.Generic;
using System.Text;
using AspectCore.DynamicProxy;

namespace AspectCore.APM.Collector
{
    [NonAspect]
    public interface ICollector : ICollectorLifetime
    {
        bool Push(IPayload payload);
    }
}