using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspectCore.Configuration;
using AspectCore.DynamicProxy;
using AspectCore.Extensions.Reflection;
using AspectCore.Injector;
using Butterfly.OpenTracing;

namespace Butterfly.Client.Tracing
{
    public class TraceAttribute : AbstractInterceptorAttribute
    {
        private static readonly List<string> excepts = new List<string>
        {
            "Microsoft.Extensions.Logging",
            "Microsoft.Extensions.Options",
            "IServiceProvider",
            "IHttpContextAccessor",
            "ITelemetryInitializer",
            "IHostingEnvironment",
            "Autofac.*",
            "Autofac",
            "Butterfly.*"
        };

        [FromContainer]
        public IServiceTracer ServiceTracer { get; set; }

        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            var serviceType = context.ServiceMethod.DeclaringType;
            if (excepts.Any(x => serviceType.Name.Matches(x)) || excepts.Any(x => serviceType.Namespace.Matches(x)) || context.Implementation is IServiceTracer)
            {
                await context.Invoke(next);
                return;
            }
            await ServiceTracer?.ChildTraceAsync(context.ServiceMethod.GetReflector().DisplayName, DateTimeOffset.UtcNow, async span =>
               {
                   span.Log(LogField.CreateNew().MethodExecuting());
                   span.Tags.Set("ServiceType", context.ServiceMethod.DeclaringType.GetReflector().FullDisplayName);
                   span.Tags.Set("ImplementationType", context.ImplementationMethod.DeclaringType.GetReflector().FullDisplayName);
                   await context.Invoke(next);
                   span.Log(LogField.CreateNew().MethodExecuted());
               });
        }
    }
}