using AspectCore.DynamicProxy;
using StackExchange.Redis;

namespace AspectCore.APM.RedisProfiler
{
    [NonAspect]
    public interface IConnectionMultiplexerProvider
    {
        IConnectionMultiplexer ConnectionMultiplexer { get; }
    }
}