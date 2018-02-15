using System.Net.Http;
using AspectCore.Configuration;
using AspectCore.DynamicProxy;
using AspectCore.Extensions.DependencyInjection;
using AspectCore.Injector;
using Butterfly.Client.Console;
using Butterfly.Client.Tracing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Butterfly.Client.Sample.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddButterfly(option =>
            {
                option.CollectorUrl = "http://localhost:9618";
                option.Service = "Console";
            });

            services.AddSingleton<HttpClient>(p => new HttpClient(p.GetService<HttpTracingHandler>()));
            services.AddSingleton<IFooService, FooService>();

            var provider = services.BuildAspectCoreServiceProvider();

            var fooService = provider.GetService<IFooService>();
            var result = fooService.GetValues();

            System.Console.WriteLine($"Get result from backend:{result}");

            System.Console.ReadLine();

            TracerManager.Init(new ButterflyOptions()
            {
                CollectorUrl = "http://localhost:9618",
                Service = "Console"
            }, new ButterflyLoggerFactory(new LoggerFactory()));

            var serviceTracer = TracerManager.Instence.ServiceTracer;
            var httpClient = new HttpClient(new HttpTracingHandler(serviceTracer));

            var proxyGenerator = new ProxyGeneratorBuilder()
                .ConfigureService(opt => opt.AddInstance(serviceTracer))
                .Build();

            var fooService2 = proxyGenerator.CreateInterfaceProxy<IFooService, FooService>(httpClient);

            var result2 = fooService2.GetValues();

            System.Console.WriteLine($"Get result from backend:{result2}");

            System.Console.ReadLine();
        }
    }
}