using System;
using System.Collections.Generic;
using System.Text;

namespace AspectCore.APM.Profiler
{
    public interface IProfilingContext
    {
        string ProfilerName { get; }
    }
}
