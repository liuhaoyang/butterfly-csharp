using Microsoft.AspNetCore.Builder;

namespace Butterfly.Client.AspNetCore
{
    public static class TracingMiddlewareExtensions
    {
        public static IApplicationBuilder UseTracing(this IApplicationBuilder app)
        {
            return app?.UseMiddleware<TracingMiddleware>();
        }
    }
}