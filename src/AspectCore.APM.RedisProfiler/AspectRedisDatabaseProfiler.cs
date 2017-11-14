using StackExchange.Redis;

namespace AspectCore.APM.RedisProfiler
{
    public sealed class AspectRedisDatabaseProfiler : IProfiler
    {
        public object GetContext()
        {
            return AspectRedisDatabaseProfilerContext.Context;
        }
    }
}