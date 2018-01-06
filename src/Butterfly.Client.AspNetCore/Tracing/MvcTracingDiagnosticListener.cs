using Butterfly.OpenTracing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DiagnosticAdapter;

namespace Butterfly.Client.AspNetCore
{
    public class MvcTracingDiagnosticListener : ITracingDiagnosticListener
    {
        public string ListenerName { get; } = "Microsoft.AspNetCore";

        [DiagnosticName("Microsoft.AspNetCore.Mvc.BeforeAction")]
        public void BeforeAction(ActionDescriptor actionDescriptor, HttpContext httpContext, RouteData routeData)
        {
            Event(httpContext, "Microsoft.AspNetCore.Mvc.BeforeAction");
        }

        [DiagnosticName("Microsoft.AspNetCore.Mvc.AfterAction")]
        public void AfterAction(ActionDescriptor actionDescriptor, HttpContext httpContext, RouteData routeData)
        {
            Event(httpContext, "Microsoft.AspNetCore.Mvc.AfterAction");
        }

        [DiagnosticName("Microsoft.AspNetCore.Mvc.BeforeOnException")]
        public void BeforeOnException(ExceptionContext exceptionContext)
        {
            var httpContext = exceptionContext.HttpContext;
            Event(httpContext, "Microsoft.AspNetCore.Mvc.BeforeOnException");
            var span = httpContext.GetSpan();
            span?.Log(LogField.CreateNew().EventError().ErrorKind(exceptionContext.Exception).ErrorObject(exceptionContext.Exception).Stack(exceptionContext.Exception.StackTrace));
        }

        [DiagnosticName("Microsoft.AspNetCore.Mvc.AfterOnException")]
        public void AfterOnException(ExceptionContext exceptionContext)
        {
            Event(exceptionContext.HttpContext, "Microsoft.AspNetCore.Mvc.AfterOnException");
        }

        private void Event(HttpContext httpContext, string @event)
        {
            var span = httpContext.GetSpan();
            span?.Log(LogField.CreateNew().Event(@event));
        }
    }
}