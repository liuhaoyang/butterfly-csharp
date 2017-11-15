using System;
using AspectCore.Configuration;
using AspectCore.Injector;
using StackExchange.Redis;

namespace AspectCore.APM.RedisProfiler
{
    public static class RedisProfiledExtensions
    {
        public static IServiceContainer AddRedisProfiler(this IServiceContainer services, IConnectionMultiplexer connectionMultiplexer)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (connectionMultiplexer == null)
            {
                throw new ArgumentNullException(nameof(connectionMultiplexer));
            }
            connectionMultiplexer.RegisterProfiler(new AspectRedisDatabaseProfiler());
            services.AddInstance<IConnectionMultiplexer>(connectionMultiplexer);
            services.AddType<IRedisProfilerCallbackHandler, RedisProfilerCallbackHandler>();
            services.Configure(ConfigureRedisProfiler);
            return services;
        }

        public static void ConfigureRedisProfiler(this IAspectConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            configuration.Interceptors.AddTyped<RedisProfiledInterceptor>(
                 Predicates.ForService(typeof(IRedis).FullName),
                 Predicates.ForService(typeof(IRedisAsync).FullName),
                 Predicates.ForService(typeof(IDatabase).FullName),
                 Predicates.ForService(typeof(IDatabaseAsync).FullName),
                 Predicates.ForService(typeof(IServer).FullName),
                 Predicates.ForService(typeof(ISubscriber).FullName));
            configuration.Interceptors.AddTyped<DatabaseProxyInterceptor>(
                Predicates.ForMethod(typeof(IConnectionMultiplexer).FullName, nameof(IConnectionMultiplexer.GetDatabase)));
            configuration.Interceptors.AddTyped<ServerProxyInterceptor>(
                Predicates.ForMethod(typeof(IConnectionMultiplexer).FullName, nameof(IConnectionMultiplexer.GetServer)));
            configuration.Interceptors.AddTyped<SubscriberProxyInterceptor>(
                Predicates.ForMethod(typeof(IConnectionMultiplexer).FullName, nameof(IConnectionMultiplexer.GetSubscriber)));
        }
    }
}