using System;
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

        public ServiceTracer(ITracer tracer, IHostingEnvironment hostingEnvironment,IOptions<ButterflyOptions> options)
        {
            _tracer = tracer ?? throw new ArgumentNullException(nameof(tracer));
            _hostingEnvironment = hostingEnvironment;
            _butterflyOptions = options.Value;
        }

        public ITracer Tracer => _tracer;

        public string ServiceName => _butterflyOptions.Service;
        
        public ISpan Start(ISpanBuilder spanBuilder)
        {
            var span = _tracer.Start(spanBuilder);

            span.Tags.Service(_butterflyOptions.Service)
                .Set("application", _hostingEnvironment.ApplicationName)
                .Set("environment", _hostingEnvironment.EnvironmentName);
            
            return span;
        }
    }
}