using System;
using System.Collections.Generic;
using System.Linq;
using Butterfly.Client.Tracing;
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

        public ISpan OnBeginRequest(HttpContext httpContext)
        {
            var spanBuilder = new SpanBuilder($"server {httpContext.Request.Method} {httpContext.Request.Path}");
            if (_tracer.Tracer.TryExtract(out var spanContext, httpContext.Request.Headers, (c, k) => c[k].GetValue(),
                c => c.Select(x => new KeyValuePair<string, string>(x.Key, x.Value.GetValue())).GetEnumerator()))
            {
                spanBuilder.AsChildOf(spanContext);
            }
            var span = _tracer.Start(spanBuilder);        
            httpContext.SetSpan(span);         
            span.Log(LogField.CreateNew().ServerReceive());
            span.Log(LogField.CreateNew().Event("AspNetCore BeginRequest"));
            span.Tags
             .Server().Component("AspNetCore")
             .HttpMethod(httpContext.Request.Method)
             .HttpUrl($"{httpContext.Request.Scheme}://{httpContext.Request.Host.ToUriComponent()}{httpContext.Request.Path}{httpContext.Request.QueryString}")
             .HttpHost(httpContext.Request.Host.ToUriComponent())
             .HttpPath(httpContext.Request.Path)
             .HttpStatusCode(httpContext.Response.StatusCode)
             .PeerAddress(httpContext.Connection.RemoteIpAddress.ToString())
             .PeerPort(httpContext.Connection.RemotePort);
            _tracer.Tracer.SetCurrentSpan(span);
            return span;
        }

        public void OnEndRequest(HttpContext httpContext)
        {
            var span = httpContext.GetSpan();
            if (span == null)
            {
                return;
            }

            span.Tags.HttpStatusCode(httpContext.Response.StatusCode);

            span.Log(LogField.CreateNew().Event("AspNetCore EndRequest"));
            span.Log(LogField.CreateNew().ServerSend());
            span.Finish();
            _tracer.Tracer.SetCurrentSpan(null);
        }

        public void OnException(HttpContext httpContext, Exception exception, string @event)
        {
            var span = httpContext.GetSpan();
            if (span == null)
            {
                return;
            }
            span?.Log(LogField.CreateNew().Event(@event));
            span?.Exception(exception);
        }
    }
}