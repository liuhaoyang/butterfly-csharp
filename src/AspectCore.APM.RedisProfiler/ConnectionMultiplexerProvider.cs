using System;
using AspectCore.APM.Core;
using AspectCore.DynamicProxy;
using StackExchange.Redis;

namespace AspectCore.APM.RedisProfiler
{
    public class ConnectionMultiplexerProvider : IConnectionMultiplexerProvider
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public IConnectionMultiplexer ConnectionMultiplexer => _connectionMultiplexer;

        public ConnectionMultiplexerProvider(IProxyGenerator proxyGenerator, IOptionAccessor<RedisProfilingOptions> optionAccessor)
        {
            if (optionAccessor == null)
            {
                throw new ArgumentNullException(nameof(IOptionAccessor<RedisProfilingOptions>));
            }
            var connectionMultiplexer = StackExchange.Redis.ConnectionMultiplexer.Connect(optionAccessor.Value.GetConfigurationOptions());
            connectionMultiplexer.RegisterProfiler(new AspectRedisDatabaseProfiler());
            _connectionMultiplexer = proxyGenerator.CreateInterfaceProxy<IConnectionMultiplexer>(connectionMultiplexer);
        }
    }
}