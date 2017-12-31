using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Butterfly.OpenTracing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Butterfly.Client.AspNetCore
{
    public class TracingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceTracer _tracer;

        public TracingMiddleware(RequestDelegate next, IServiceTracer tracer)
        {
            _next = next;
            _tracer = tracer ?? throw new ArgumentNullException(nameof(tracer));
        }

        public Task Invoke(HttpContext httpContext)
        {
            var spanBuilder = new SpanBuilder($"server {httpContext.Request.Method} {httpContext.Request.Path}");
            if (_tracer.Tracer.TryExtract(out var spanContext, httpContext.Request.Headers, (c, k) => c[k],
                c => c.Select(x => new KeyValuePair<string, string>(x.Key, x.Value)).GetEnumerator()))
            {
                spanBuilder.AsChildOf(spanContext);
            }

            return _tracer.TraceAsync(spanBuilder, async span =>
            {
                span.Tags
                    .Server().Component("AspNetCore")
                    .HttpMethod(httpContext.Request.Method)
                    .HttpUrl($"{httpContext.Request.Scheme}://{httpContext.Request.Host.ToUriComponent()}{httpContext.Request.Path}{httpContext.Request.QueryString}")
                    .HttpHost(httpContext.Request.Host.ToUriComponent())
                    .HttpPath(httpContext.Request.Path)
                    .PeerAddress(httpContext.Connection.RemoteIpAddress.ToString())
                    .PeerPort(httpContext.Connection.RemotePort);

                span.Log(LogField.CreateNew().ServerReceive());

                await _next(httpContext);

                span.Log(LogField.CreateNew().ServerSend());

                span.Tags.HttpStatusCode(httpContext.Response.StatusCode);

            });
        }
    }
}