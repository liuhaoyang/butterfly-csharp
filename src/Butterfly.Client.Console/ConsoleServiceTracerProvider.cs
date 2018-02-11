using System;
using Butterfly.Client.Tracing;
using Butterfly.OpenTracing;
using Microsoft.Extensions.Options;
using System.Net;

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
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? Environment.MachineName;
            return new ServiceTracer(_tracer, _options.Service, environmentName, AppDomain.CurrentDomain.FriendlyName, Dns.GetHostName());
        }
    }
}