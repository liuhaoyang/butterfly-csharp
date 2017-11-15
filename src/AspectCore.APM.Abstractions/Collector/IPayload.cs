using System;
using System.Collections.Generic;
using System.Text;

namespace AspectCore.APM.Collector
{
    public interface IPayload : IReadOnlyList<IPoint>
    {
    }
}