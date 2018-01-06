using System;
using System.Collections.Generic;
using System.Linq;
using Butterfly.OpenTracing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DiagnosticAdapter;

namespace Butterfly.Client.AspNetCore
{
    public class TracingDiagnosticListener : ITracingDiagnosticListener
    {
        private readonly IRequestTracer _requestTracer;

        public TracingDiagnosticListener(IRequestTracer requestTracer)
        {
            _requestTracer = requestTracer;
        }

        public string ListenerName { get; } = "Microsoft.AspNetCore";
        
        [DiagnosticName("Microsoft.AspNetCore.Hosting.HttpRequestIn")]
        public void OnHttpRequestIn()
        {
            // do nothing, just enable the diagnotic source
        }

        [DiagnosticName("Microsoft.AspNetCore.Hosting.HttpRequestIn.Start")]
        public void OnHttpRequestInStart(HttpContext httpContext)
        {
            _requestTracer.OnBeginRequest(httpContext);
        }
        
        [DiagnosticName("Microsoft.AspNetCore.Hosting.HttpRequestIn.Stop")]
        public void OnHttpRequestInStop(HttpContext httpContext)
        {
            _requestTracer.OnEndRequest(httpContext);
        }

        [DiagnosticName("Microsoft.AspNetCore.Diagnostics.HandledException")]
        public void DiagnosticHandledException(HttpContext httpContext, Exception exception)
        {
            _requestTracer.OnException(httpContext, exception, "Microsoft.AspNetCore.Diagnostics.HandledException");
        }

        [DiagnosticName("Microsoft.AspNetCore.Diagnostics.UnhandledException")]
        public void DiagnosticUnhandledException(HttpContext httpContext, Exception exception)
        {
            _requestTracer.OnException(httpContext, exception, "Microsoft.AspNetCore.Diagnostics.UnhandledException");
        }

        [DiagnosticName("Microsoft.AspNetCore.Hosting.UnhandledException")]
        public void HostingUnhandledException(HttpContext httpContext, Exception exception)
        {
            _requestTracer.OnException(httpContext, exception, "Microsoft.AspNetCore.Hosting.UnhandledException");
        }
    }
}