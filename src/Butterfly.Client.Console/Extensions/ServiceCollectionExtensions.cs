using System;
using AspectCore.Configuration;
using AspectCore.Extensions.DependencyInjection;
using Butterfly.Client.Tracing;
using Butterfly.OpenTracing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Butterfly.Client.Console
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddButterfly(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddButterfly().Configure<ButterflyOptions>(configuration);
        }

        public static IServiceCollection AddButterfly(this IServiceCollection services, Action<ButterflyOptions> configure)
        {
            var option = new ButterflyOptions();
            configure(option);
            services.AddSingleton<IOptions<ButterflyOptions>>(new OptionsWrapper<ButterflyOptions>(option));
            return services.AddButterfly().Configure(configure);
        }

        private static IServiceCollection AddButterfly(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddTransient<HttpTracingHandler>();
            services.AddSingleton<ISpanContextFactory, SpanContextFactory>();
            services.AddSingleton<ISampler, FullSampler>();
            services.AddSingleton<ITracer, Tracer>();
            services.AddSingleton<IServiceTracerProvider, ConsoleServiceTracerProvider>();
            services.AddSingleton<IServiceTracer>(provider => provider.GetRequiredService<IServiceTracerProvider>().GetServiceTracer());
            services.AddSingleton<ISpanRecorder, AsyncSpanRecorder>();
            services.AddSingleton<IButterflyDispatcherProvider, ButterflyDispatcherProvider>();
            services.AddSingleton<IButterflyDispatcher>(provider => provider.GetRequiredService<IButterflyDispatcherProvider>().GetDispatcher());
            services.AddSingleton<IButterflySenderProvider, ButterflySenderProvider>();
            services.AddSingleton<IDispatchCallback, SpanDispatchCallback>();
            services.AddSingleton<ITraceIdGenerator, TraceIdGenerator>();

            services.AddSingleton<Logging.ILoggerFactory>(new ButterflyLoggerFactory(new LoggerFactory()));

            services.AddDynamicProxy(option =>
            {
                option.NonAspectPredicates.AddNamespace("Butterfly.*");
            });

            return services;
        }
    }
}