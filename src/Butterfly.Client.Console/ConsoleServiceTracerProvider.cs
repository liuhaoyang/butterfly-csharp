using System;
using System.Diagnostics;
using Butterfly.Client.Tracing;
using Butterfly.OpenTracing;
using Microsoft.Extensions.Options;
using System.Net;
using System.Reflection;

namespace Butterfly.Client.Console
{
    public class ConsoleServiceTracerProvider : IServiceTracerProvider
    {
        private readonly ITracer _tracer;
        private readonly ButterflyOptions _options;

        public ConsoleServiceTracerProvider(ITracer tracer, IOptions<ButterflyOptions> options)
        {
            _tracer = tracer;
            _options = options.Value;
        }

        public IServiceTracer GetServiceTracer()
        {
            var environmentName = Assembly.GetEntryAssembly().GetCustomAttribute<DebuggableAttribute>().IsJITTrackingEnabled ? "Development" : "Production";
            var service = _options.Service ?? Assembly.GetEntryAssembly().GetName().Name;
            var host = Dns.GetHostName();
            var identity = string.IsNullOrEmpty(_options.ServiceIdentity) ? $"{service}@{host}" : _options.ServiceIdentity;

            return new ServiceTracer(_tracer, service, environmentName, identity, host);
        }
    }
}