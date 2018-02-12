using System.Net.Http;
using AspectCore.Extensions.DependencyInjection;
using Butterfly.Client.Console;
using Butterfly.Client.Tracing;
using Microsoft.Extensions.DependencyInjection;

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
        }
    }
}