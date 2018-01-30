using System;
using System.Net;
using Butterfly.Client.Tracing;
using Butterfly.OpenTracing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace Butterfly.Client.AspNetCore
{
    public class ServiceTracer : IServiceTracer
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ButterflyOptions _butterflyOptions;
        private readonly ITracer _tracer;
        private readonly string _hostName;

        public ServiceTracer(ITracer tracer, IHostingEnvironment hostingEnvironment, IOptions<ButterflyOptions> options)
        {
            _tracer = tracer ?? throw new ArgumentNullException(nameof(tracer));
            _hostingEnvironment = hostingEnvironment;
            _butterflyOptions = options.Value;
            _hostName = Dns.GetHostName();
        }

        public ITracer Tracer => _tracer;

        public string ServiceName => _butterflyOptions.Service;

        public ISpan Start(ISpanBuilder spanBuilder)
        {
            var span = _tracer.Start(spanBuilder);

            span.Tags.Service(_butterflyOptions.Service)
                .ServiceApplicationName(_hostingEnvironment.ApplicationName)
                .ServiceEnvironment(_hostingEnvironment.EnvironmentName)
                .ServiceHost(_hostName);

            return new ChildSpan(span, _tracer);
        }
    }
}