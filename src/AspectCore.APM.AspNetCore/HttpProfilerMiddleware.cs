using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AspectCore.APM.HttpProfiler;
using AspectCore.APM.Profiler;
using AspectCore.Injector;
using Microsoft.AspNetCore.Http;

namespace AspectCore.APM.AspNetCore
{
    public class HttpProfilerMiddleware
    {
        private readonly RequestDelegate _next;

        public HttpProfilerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            int? statusCode = default(int?);
            var profilers = httpContext.RequestServices.ResolveMany<IProfiler<HttpProfilingContext>>();
            if (!profilers.Any())
            {
                await _next(httpContext);
                return;
            }
            Stopwatch stopwatch = Stopwatch.StartNew();
            try
            {
                await _next(httpContext);
                statusCode = httpContext.Response.StatusCode;
            }
            catch
            {
                statusCode = 500;
                throw;
            }
            finally
            {
                stopwatch.Stop();
                var context = new HttpProfilingContext
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
                    StatusCode = statusCode?.ToString(),
                };
                foreach (var profiler in profilers)
                    await profiler.Invoke(context);
            }
        }
    }
}