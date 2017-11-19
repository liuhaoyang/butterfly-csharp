using AspectCore.APM.Profiler;

namespace AspectCore.APM.ApplicationProfiler
{
    public class ApplicationGCProfilingContext : IProfilingContext
    {
        public string ProfilerName => "application_gc";
    }
}
