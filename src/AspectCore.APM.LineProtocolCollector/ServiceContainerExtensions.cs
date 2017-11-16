using System;
using AspectCore.APM.Collector;
using AspectCore.Injector;

namespace AspectCore.APM.LineProtocolCollector
{
    public static class ServiceContainerExtensions
    {
        public static IServiceContainer AddLineProtocolCollector(this IServiceContainer services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            services.AddType<IPayloadClientProvider, LineProtocolPayloadClientProvider>(Lifetime.Singleton);
            return services;
        }
    }
}