using System.Security.Authentication.ExtendedProtection;
using System;
using Microsoft.Extensions.DependencyInjection;
using Butterfly.Client.Console;
using System.Net.Http;
using Butterfly.Client.Tracing;

namespace Butterfly.Client.Sample.Console
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

            var provider = services.BuildServiceProvider();

            var httpClient = provider.GetService<HttpClient>();

            var result = httpClient.GetStringAsync("http://localhost:5002/api/values").Result;

            System.Console.WriteLine($"Get result from backend:{result}");

            System.Console.ReadLine();
        }
    }
}