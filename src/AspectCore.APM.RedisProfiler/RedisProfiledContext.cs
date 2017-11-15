using AspectCore.APM.Profiler;

namespace AspectCore.APM.RedisProfiler
{
    public sealed class RedisProfiledContext : IProfiledContext
    {
        public string ProfilerName => "redis_client";
    }
}