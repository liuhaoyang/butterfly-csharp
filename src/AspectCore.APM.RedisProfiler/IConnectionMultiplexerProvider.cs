using StackExchange.Redis;

namespace AspectCore.APM.RedisProfiler
{
    public interface IConnectionMultiplexerProvider
    {
        IConnectionMultiplexer ConnectionMultiplexer { get; }
    }
}