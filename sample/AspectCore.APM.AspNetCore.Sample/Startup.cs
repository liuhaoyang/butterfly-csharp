using System;
using AspectCore.APM.ApplicationProfiler;
using AspectCore.APM.HttpProfiler;
using AspectCore.APM.LineProtocolCollector;
using AspectCore.APM.RedisProfiler;
using AspectCore.Configuration;
using AspectCore.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AspectCore.APM.MethodProfiler;
using AspectCore.APM.AspNetCore.Sample.Services;

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

            services.AddTransient<IContactService, ContactService>();

            services.AddAspectCoreAPM(component =>
            {
                component.AddLineProtocolCollector(options => Configuration.GetLineProtocolSection().Bind(options))
                         .AddHttpProfiler()
                         .AddApplicationProfiler()
                         .AddRedisProfiler(options => Configuration.GetRedisProfilerSection().Bind(options));
                component.AddMethodProfiler();
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
