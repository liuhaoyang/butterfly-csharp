using System;
using System.Collections.Generic;
using System.Linq;
using Butterfly.OpenTracing;
using Microsoft.AspNetCore.Http;

namespace Butterfly.Client.AspNetCore
{
    public class RequestTracer : IRequestTracer
    {
        private readonly IServiceTracer _tracer;

        public RequestTracer(IServiceTracer tracer)
        {
            _tracer = tracer;
        }

        public ISpan Span { get; private set; }

        public string TraceId
        {
            get { return Span?.SpanContext?.TraceId; }
        }

        public void OnBeginRequest(HttpContext httpContext)
        {
            var spanBuilder = new SpanBuilder($"server {httpContext.Request.Method} {httpContext.Request.Path}");
            if (_tracer.Tracer.TryExtract(out var spanContext, httpContext.Request.Headers, (c, k) => c[k],
                c => c.Select(x => new KeyValuePair<string, string>(x.Key, x.Value)).GetEnumerator()))
            {
                spanBuilder.AsChildOf(spanContext);
            }

            Span = _tracer.Start(spanBuilder);
            Span.Log(LogField.CreateNew().ServerReceive());
            Span.Log(LogField.CreateNew().Event("Microsoft.AspNetCore.Hosting.BeginRequest"));
            _tracer.Tracer.SetCurrentSpan(Span);
            httpContext.SetSpan(Span);
        }

        public void OnEndRequest(HttpContext httpContext)
        {
            if (Span == null)
            {
                return;
            }

            Span.Tags
                .RequestMetrics().Server().Component("AspNetCore")
                .HttpMethod(httpContext.Request.Method)
                .HttpUrl($"{httpContext.Request.Scheme}://{httpContext.Request.Host.ToUriComponent()}{httpContext.Request.Path}{httpContext.Request.QueryString}")
                .HttpHost(httpContext.Request.Host.ToUriComponent())
                .HttpPath(httpContext.Request.Path)
                .HttpStatusCode(httpContext.Response.StatusCode)
                .PeerAddress(httpContext.Connection.RemoteIpAddress.ToString())
                .PeerPort(httpContext.Connection.RemotePort);
            Span.Log(LogField.CreateNew().Event("Microsoft.AspNetCore.Hosting.EndRequest"));
            Span.Log(LogField.CreateNew().ServerSend());
            Span.Finish();
            _tracer.Tracer.SetCurrentSpan(null);
        }

        public void OnException(HttpContext httpContext, Exception exception, string @event)
        {
            Span?.Log(LogField.CreateNew().Event(@event));
            Span?.Exception(exception);
        }
    }
}