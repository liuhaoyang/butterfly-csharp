using System;
using AspectCore.APM.Core;
using AspectCore.Extensions.DependencyInjection;
using AspectCore.Injector;
using Microsoft.Extensions.DependencyInjection;

namespace AspectCore.APM.AspNetCore
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAspectCoreAPM(this IServiceCollection services, Action<ComponentOptions> componentOptions, Action<ApplicationOptions> applicationOptions = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (componentOptions == null)
            {
                throw new ArgumentNullException(nameof(componentOptions));
            }

            var apmComponent = new ComponentOptions();
            apmComponent.AddAPMCore(applicationOptions);
            componentOptions(apmComponent);

            foreach (var service in apmComponent.Services)
            {
                var descriptor = GetServiceDescriptor(service);
                if (descriptor != null)
                    services.Add(descriptor);
            }

            services.AddDynamicProxy(config =>
            {
                foreach (var interceptor in apmComponent.Services.Configuration.Interceptors)
                    config.Interceptors.Add(interceptor);
            });

            services.AddTransient<IInternalLogger, InternalLogger>();

            return services;
        }

        private static ServiceDescriptor GetServiceDescriptor(ServiceDefinition service)
        {
            if (service is TypeServiceDefinition typeService)
            {
                return ServiceDescriptor.Describe(typeService.ServiceType, typeService.ImplementationType, GetLifetime(typeService.Lifetime));
            }
            else if (service is DelegateServiceDefinition delegateService)
            {
                return ServiceDescriptor.Describe(delegateService.ServiceType, p => delegateService.ImplementationDelegate(p.GetRequiredService<IServiceResolver>()), GetLifetime(delegateService.Lifetime));
            }
            else
            {
                var instanceService = service as InstanceServiceDefinition;
                if (instanceService != null)
                {
                    return ServiceDescriptor.Singleton(instanceService.ServiceType, instanceService.ImplementationInstance);
                }
            }
            return null;
        }

        private static ServiceLifetime GetLifetime(Lifetime lifetime)
        {
            switch (lifetime)
            {
                case Lifetime.Scoped:
                    return ServiceLifetime.Scoped;
                case Lifetime.Singleton:
                    return ServiceLifetime.Singleton;
                default:
                    return ServiceLifetime.Transient;
            }
        }
    }
}