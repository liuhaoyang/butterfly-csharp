using AspectCore.APM.Profiler;

namespace AspectCore.APM.RedisProfiler
{
    internal sealed class RedisProfiledContext : IProfiledContext
    {
        public string ProfilerName => "redis_client";
    }
}