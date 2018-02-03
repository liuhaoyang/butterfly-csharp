using System;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using AspectCore.Injector;
using AspectCore.Extensions.Reflection;
using Butterfly.OpenTracing;

namespace Butterfly.Client.Tracing
{
    public class TraceAttribute : AbstractInterceptorAttribute
    {
        [FromContainer]
        public IServiceTracer ServiceTracer { get; set; }

        public override Task Invoke(AspectContext context, AspectDelegate next)
        {
            return ServiceTracer?.ChildTraceAsync(context.ServiceMethod.GetReflector().DisplayName, DateTimeOffset.UtcNow, async span =>
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