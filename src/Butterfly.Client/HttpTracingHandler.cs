using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Butterfly.OpenTracing;

namespace Butterfly.Client
{
    public class HttpTracingHandler : DelegatingHandler
    {
        private readonly string _serviceName;
        private readonly ITracer _tracer;

        public HttpTracingHandler(ITracer tracer, string serviceName, HttpMessageHandler httpMessageHandler = null)
        {
            _serviceName = serviceName;
            _tracer = tracer ?? throw new ArgumentNullException(nameof(tracer));
            InnerHandler = httpMessageHandler ?? new HttpClientHandler();
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return _tracer.ChildTraceAsync("http client", DateTimeOffset.UtcNow, (tracer, span) => TracingSendAsync(tracer, span, request, cancellationToken));
        }

        protected virtual async Task<HttpResponseMessage> TracingSendAsync(ITracer tracer, ISpan span, HttpRequestMessage request, CancellationToken cancellationToken)
        {
            span.Tags.Service(_serviceName)
                .Client().Component("HttpClient")
                .HttpMethod(request.Method.Method)
                .HttpUrl(request.RequestUri.OriginalString)
                .HttpHost(request.RequestUri.Host)
                .HttpPath(request.RequestUri.PathAndQuery)
                .PeerAddress(request.RequestUri.OriginalString)
                .PeerHostName(request.RequestUri.Host)
                .PeerPort(request.RequestUri.Port);

            tracer.Inject(span.SpanContext, request.Headers, (c, k, v) => c.Add(k, v));

            span.Log(LogField.CreateNew().ClientSend());

            var responseMessage = await base.SendAsync(request, cancellationToken);

            span.Log(LogField.CreateNew().ClientSend());

            return responseMessage;
        }
    }
}