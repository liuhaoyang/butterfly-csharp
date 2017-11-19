using AspectCore.APM.Profiler;

namespace AspectCore.APM.ApplicationProfiler
{
    public class ApplicationThreadingProfilingContext : IProfilingContext
    {
        public string ProfilerName => "application_threading";
    }
}
