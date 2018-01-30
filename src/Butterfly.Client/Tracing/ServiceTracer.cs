using System;
using System.Net;
using Butterfly.Client.Tracing;
using Butterfly.OpenTracing;

namespace Butterfly.Client.Tracing
{
    public class ServiceTracer : IServiceTracer
    {
        private readonly ITracer _tracer;
        private readonly string _service;
        private readonly string _environmentName;
        private readonly string _applicationName;
        private readonly string _hostName;

        public ServiceTracer(ITracer tracer, string service, string environmentName, string applicationName, string hostName = null)
        {
            if (string.IsNullOrEmpty(service))
            {
                throw new ArgumentNullException(nameof(service));
            }
            if (string.IsNullOrEmpty(environmentName))
            {
                throw new ArgumentNullException(nameof(environmentName));
            }
            if (string.IsNullOrEmpty(applicationName))
            {
                throw new ArgumentNullException(nameof(applicationName));
            }
            if (tracer == null)
            {
                throw new ArgumentNullException(nameof(tracer));
            }
            _tracer = tracer;
            _service = service;
            _environmentName = environmentName;
            _applicationName = applicationName;
            _hostName = hostName;
        }

        public ITracer Tracer => _tracer;

        public string ServiceName => _service;

        public string EnvironmentName => throw new NotImplementedException();

        public ISpan Start(ISpanBuilder spanBuilder)
        {
            var span = _tracer.Start(spanBuilder);

            span.Tags.Service(_service)
                .ServiceApplicationName(_applicationName)
                .ServiceEnvironment(_environmentName)
                .ServiceHost(_hostName);

            return new ServiceSpan(span, _tracer);
        }
    }
}