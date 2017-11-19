using AspectCore.APM.Profiler;

namespace AspectCore.APM.HttpProfiler
{
    internal class HttpProfilingContext : IProfilingContext
    {
        public string ProfilerName => "http_req";
    }
}
