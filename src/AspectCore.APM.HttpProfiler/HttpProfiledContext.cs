using System;
using System.Collections.Generic;
using System.Text;
using AspectCore.APM.Profiler;

namespace AspectCore.APM.HttpProfiler
{
    internal class HttpProfiledContext : IProfiledContext
    {
        public string ProfilerName => "http_pipeline";
    }
}
