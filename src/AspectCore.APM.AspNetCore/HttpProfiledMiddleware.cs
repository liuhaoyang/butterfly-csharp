using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AspectCore.APM.Common;
using AspectCore.APM.HttpProfiler;
using AspectCore.APM.Profiler;
using AspectCore.Injector;
using Microsoft.AspNetCore.Http;

namespace AspectCore.APM.AspNetCore
{
    public class HttpProfiledMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly APMOptions _options;

        public HttpProfiledMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var callbacks = httpContext.RequestServices.ResolveMany<IProfiledCallback<HttpProfiledCallbackContext>>();
            if (!callbacks.Any())
            {
                await _next(httpContext);
                return;
            }
            Stopwatch stopwatch = Stopwatch.StartNew();
            await _next(httpContext);
            stopwatch.Stop();
            var callbackContext = new HttpProfiledCallbackContext
            {
                Elapsed = stopwatch.ElapsedMilliseconds,
                HttpHost = httpContext.Request.Host.Host,
                HttpMethod = httpContext.Request.Method,
                HttpPath = httpContext.Request.Path,
                HttpPort = httpContext.Request.Host.Port.ToString(),
                HttpProtocol = httpContext.Request.Protocol,
                HttpScheme = httpContext.Request.Scheme,
                IdentityAuthenticationType = httpContext.User.Identity.AuthenticationType,
                IdentityName = httpContext.User.Identity.Name,
                RequestContentType = httpContext.Request.ContentType,
                ResponseContentType = httpContext.Response.ContentType,
                StatusCode = httpContext.Response.StatusCode.ToString()
            };
            foreach (var callback in callbacks)
                await callback.Invoke(callbackContext);
        }
    }
}