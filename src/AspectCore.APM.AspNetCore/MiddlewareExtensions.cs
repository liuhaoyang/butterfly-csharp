using AspectCore.APM.Collector;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace AspectCore.APM.AspNetCore
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseHttpProfiling(this IApplicationBuilder app)
        {
            return app.UseMiddleware<HttpProfilingMiddleware>();
        }

        public static IApplicationBuilder UseAspectCoreAPM(this IApplicationBuilder app)
        {
            var applicationLifetime = app.ApplicationServices.GetRequiredService<IApplicationLifetime>();
            var collectorLifetime = app.ApplicationServices.GetRequiredService<ICollectorLifetime>();
            applicationLifetime.ApplicationStarted.Register(() => collectorLifetime.Start());
            applicationLifetime.ApplicationStopping.Register(() => collectorLifetime.Stop());
            return app;
        }
    }
}