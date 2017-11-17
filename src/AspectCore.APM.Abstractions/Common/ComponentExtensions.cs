using System;
using System.Reflection;
using AspectCore.APM.Collector;
using AspectCore.Injector;

namespace AspectCore.APM.Common
{
    public static class ComponentExtensions
    {
        public static ApmComponentOptions AddAPMCore(this ApmComponentOptions apmComponent)
        {
            if (apmComponent == null)
            {
                throw new ArgumentNullException(nameof(apmComponent));
            }
            apmComponent.Services.AddType<IPayloadDispatcher, AsyncQueueDispatcher>(Lifetime.Singleton);
            apmComponent.Services.AddType<ICollectorLifetime, CollectorLifetime>(Lifetime.Singleton);
            apmComponent.Services.AddDelegate<ICollector>(r => r.Resolve<ICollectorLifetime>());
            apmComponent.Services.AddType<IPayloadSender, PayloadSender>();
            return apmComponent;
        }

        public static ApmComponentOptions AddAPMCore(this ApmComponentOptions apmComponent, Action<ApplicationOptions> configure)
        {
            if (apmComponent == null)
            {
                throw new ArgumentNullException(nameof(apmComponent));
            }

            apmComponent.AddAPMCore();
            var applicationOptions = new ApplicationOptions();

            configure?.Invoke(applicationOptions);

            if (applicationOptions.ApplicationName == null)
                applicationOptions.ApplicationName = Assembly.GetEntryAssembly().GetName().Name;
            if (applicationOptions.Environment == null)
                applicationOptions.Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? Environment.GetEnvironmentVariable("ENVIRONMENT");

            apmComponent.Services.AddInstance<IOptionAccessor<ApplicationOptions>>(applicationOptions);

            return apmComponent;
        }


        public static IServiceContainer AddAspectCoreAPM(this IServiceContainer services, Action<ApmComponentOptions> componentOptions, Action<ApplicationOptions> applicationOptions = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (componentOptions == null)
            {
                throw new ArgumentNullException(nameof(componentOptions));
            }
            var apmComponent = new ApmComponentOptions();
            apmComponent.AddAPMCore(applicationOptions);
            componentOptions(apmComponent);
            foreach (var service in apmComponent.Services)
                services.Add(service);
            foreach (var interceptor in apmComponent.Services.Configuration.Interceptors)
                services.Configuration.Interceptors.Add(interceptor);
            return services;
        }
    }
}