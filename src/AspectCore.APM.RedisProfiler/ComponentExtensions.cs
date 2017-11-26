using System;
using AspectCore.APM.Core;
using AspectCore.APM.Profiler;
using AspectCore.Configuration;
using AspectCore.Injector;
using StackExchange.Redis;

namespace AspectCore.APM.RedisProfiler
{
    public static class ComponentExtensions
    {
        public static ComponentOptions AddRedisProfiler(this ComponentOptions apmComponent, Action<RedisProfilingOptions> configure)
        {
            if (apmComponent == null)
            {
                throw new ArgumentNullException(nameof(apmComponent));
            }
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }
            var redisConfigurationOptions = new RedisProfilingOptions();
            configure(redisConfigurationOptions);
            apmComponent.Services.AddInstance<IOptionAccessor<RedisProfilingOptions>>(redisConfigurationOptions);
            apmComponent.Services.AddType<IConnectionMultiplexerProvider, ConnectionMultiplexerProvider>(Lifetime.Singleton);
            apmComponent.Services.AddDelegate<IConnectionMultiplexer>(r => r.ResolveRequired<IConnectionMultiplexerProvider>().ConnectionMultiplexer, Lifetime.Singleton);
            apmComponent.Services.Configure(ConfigureRedisProfiler);
            apmComponent.Services.AddType<IProfiler<RedisProfilingContext>, RedisProfiler>(Lifetime.Singleton);
            return apmComponent;
        }

        public static void ConfigureRedisProfiler(this IAspectConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            configuration.Interceptors.AddTyped<RedisProfilingInterceptor>(
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
            configuration.NonAspectPredicates.AddNamespace("StackExchange.Redis");
        }
    }
}