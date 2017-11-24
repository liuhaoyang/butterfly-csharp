using System;
using AspectCore.APM.Collector;
using AspectCore.APM.HttpProfiler;
using AspectCore.APM.LineProtocolCollector;
using AspectCore.Extensions.DependencyInjection;
using AspectCore.APM.ApplicationProfiler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AspectCore.APM.RedisProfiler;

namespace AspectCore.APM.AspNetCore.Sample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddAspectCoreAPM(component =>
            {
                component.AddLineProtocolCollector(options => Configuration.GetLineProtocolSection().Bind(options))
                         .AddHttpProfiler()
                         .AddApplicationProfiler()
                         .AddRedisProfiler(options => Configuration.GetRedisProfilerSection().Bind(options));
            });

            return services.BuildAspectCoreServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseHttpProfiler();

            app.UseStaticFiles();

            app.UseAspectCoreAPM();
           
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
