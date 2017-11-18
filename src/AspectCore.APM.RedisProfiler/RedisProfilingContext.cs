using AspectCore.APM.Profiler;

namespace AspectCore.APM.RedisProfiler
{
    internal sealed class RedisProfilingContext : IProfilingContext
    {
        public string ProfilerName => "redis_client";
    }
}