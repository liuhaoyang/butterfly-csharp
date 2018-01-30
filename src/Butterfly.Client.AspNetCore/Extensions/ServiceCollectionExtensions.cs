using System;
using System.IO;
using Butterfly.OpenTracing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Butterfly.Client.Tracing;

namespace Butterfly.Client.AspNetCore
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddButterfly(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddButterfly().Configure<ButterflyOptions>(configuration);
        }

        public static IServiceCollection AddButterfly(this IServiceCollection services, Action<ButterflyOptions> configure)
        {
            return services.AddButterfly().Configure<ButterflyOptions>(configure);
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
            services.AddSingleton<IServiceTracerProvider, ServiceTracerProvider>();
            services.AddSingleton<IServiceTracer>(provider => provider.GetRequiredService<IServiceTracerProvider>().GetServiceTracer());
            services.AddSingleton<ISpanRecorder, AsyncSpanRecorder>();
            services.AddSingleton<IButterflyDispatcherProvider, ButterflyDispatcherProvider>();
            services.AddSingleton<IButterflyDispatcher>(provider => provider.GetRequiredService<IButterflyDispatcherProvider>().GetDispatcher());
            services.AddSingleton<IButterflySenderProvider, ButterflySenderProvider>();
            services.AddSingleton<IHostedService, ButterflyHostedService>();
            services.AddSingleton<ITracingDiagnosticListener, TracingDiagnosticListener>();
            services.AddSingleton<ITracingDiagnosticListener, MvcTracingDiagnosticListener>();
            services.AddSingleton<IRequestTracer, RequestTracer>();
            services.AddSingleton<IDispatchCallback, SpanDispatchCallback>();
            services.AddSingleton<ITraceIdGenerator, TraceIdGenerator>();
            return services;
        }
    }
}