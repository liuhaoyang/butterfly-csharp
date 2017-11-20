using System;
using AspectCore.APM.Common;
using AspectCore.APM.LineProtocolCollector;
using AspectCore.APM.ApplicationProfiler;
using AspectCore.APM.Collector;
using AspectCore.Injector;

namespace AspectCore.APM.Sample.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceContainer services = new ServiceContainer();

            services.AddType<IInternalLogger, ConsoleLogger>();

            Action<ApmComponentOptions> componentOptions = component =>
             {
                 component.AddLineProtocolCollector(options =>
                 {
                     options.Server = "http://localhost:8186";
                     options.Database = "aspectcore_apm";
                 });
                 component.AddApplicationProfiler(op => op.Interval = 1);
             };

            Action<ApplicationOptions> applicationOptions = options =>
            {
                options.ApplicationName = typeof(Program).Assembly.GetName().Name;
                options.Environment = "Development";
            };
            services.AddAspectCoreAPM(componentOptions, applicationOptions);

            var serviceResolver = services.Build();

            var collectorLifetime = serviceResolver.Resolve<ICollectorLifetime>();

            collectorLifetime.Start();

            Console.ReadKey();

            collectorLifetime.Stop();
        }
    }

    public class ConsoleLogger : AspectCore.APM.Common.IInternalLogger
    {
        public void LogError(string message, Exception exception)
        {
            Console.WriteLine(message);
            Console.WriteLine(exception);
        }

        public void LogInformation(string message)
        {
            Console.WriteLine(message);
        }

        public void LogWarning(string message)
        {
            Console.WriteLine(message);
        }
    }
}