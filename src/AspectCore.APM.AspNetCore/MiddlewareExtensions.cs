using AspectCore.APM.Collector;
using AspectCore.APM.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace AspectCore.APM.AspNetCore
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseHttpProfiler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<HttpProfilerMiddleware>();
        }

        public static IApplicationBuilder UseAspectCoreAPM(this IApplicationBuilder app)
        {
            var applicationLifetime = app.ApplicationServices.GetRequiredService<IApplicationLifetime>();
            var collectorLifetime = app.ApplicationServices.GetRequiredService<IComponentLifetime>();
            applicationLifetime.ApplicationStarted.Register(() => collectorLifetime.Start());
            applicationLifetime.ApplicationStopping.Register(() => collectorLifetime.Stop());
            return app;
        }
    }
}