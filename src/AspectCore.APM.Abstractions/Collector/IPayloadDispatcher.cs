using System;
using System.Collections.Generic;
using System.Text;
using AspectCore.DynamicProxy;

namespace AspectCore.APM.Collector
{
    [NonAspect]
    public interface IPayloadDispatcher
    {
        string Name { get; }

        bool Dispatch(IPayload payload);

        void Start();

        void Stop();
    }
}