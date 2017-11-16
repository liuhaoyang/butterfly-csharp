using System;
using System.Collections.Generic;
using System.Text;
using AspectCore.APM.Collector;
using AspectCore.Injector;

namespace AspectCore.APM.Common
{
    public static class ServiceContainerExtensions
    {
        public static IServiceContainer AddAspectCoreAPM(this IServiceContainer services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            services.AddType<IPayloadDispatcher, AsyncQueueDispatcher>(Lifetime.Singleton);
            services.AddType<ICollectorLifetime, CollectorLifetime>(Lifetime.Singleton);
            services.AddDelegate<ICollector>(r => r.Resolve<ICollectorLifetime>());
            services.AddType<IPayloadSender, PayloadSender>();
            return services;
        }
    }
}