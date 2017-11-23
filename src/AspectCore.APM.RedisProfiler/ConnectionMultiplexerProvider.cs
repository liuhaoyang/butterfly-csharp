using System;
using AspectCore.APM.Core;
using StackExchange.Redis;

namespace AspectCore.APM.RedisProfiler
{
    public class ConnectionMultiplexerProvider : IConnectionMultiplexerProvider
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public IConnectionMultiplexer ConnectionMultiplexer => _connectionMultiplexer;

        public ConnectionMultiplexerProvider(IOptionAccessor<RedisProfilingOptions> optionAccessor)
        {
            if (optionAccessor == null)
            {
                throw new ArgumentNullException(nameof(IOptionAccessor<RedisProfilingOptions>));
            }
            _connectionMultiplexer = StackExchange.Redis.ConnectionMultiplexer.Connect(optionAccessor.Value.GetConfigurationOptions());
        }
    }
}
