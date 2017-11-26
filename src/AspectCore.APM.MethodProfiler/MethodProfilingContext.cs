using System;
using System.Collections.Generic;
using System.Text;

namespace AspectCore.APM.MethodProfiler
{
    public class MethodProfilingContext : Profiler.IProfilingContext
    {
        public string ProfilerName => "method_exec";

        public long ElapsedMicroseconds { get; set; }

        public string MethodName { get; set; }

        public string Namespace { get; set; }

        public string ServiceType { get; set; }

        public string ImplementationType { get; set; }
    }
}
