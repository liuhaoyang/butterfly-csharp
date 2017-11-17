using System;
using AspectCore.APM.Common;
using StackExchange.Redis;

namespace AspectCore.APM.RedisProfiler
{
    public class ConnectionMultiplexerProvider : IConnectionMultiplexerProvider
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public IConnectionMultiplexer ConnectionMultiplexer => _connectionMultiplexer;

        public ConnectionMultiplexerProvider(IOptionAccessor<RedisConfigurationOptions> optionAccessor)
        {
            if (optionAccessor == null)
            {
                throw new ArgumentNullException(nameof(IOptionAccessor<RedisConfigurationOptions>));
            }
            _connectionMultiplexer = StackExchange.Redis.ConnectionMultiplexer.Connect(optionAccessor.Value.Copy());
        }
    }
}
