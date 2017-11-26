using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AspectCore.APM.Core;
using AspectCore.APM.Profiler;
using AspectCore.DynamicProxy;
using AspectCore.Extensions.Reflection;
using AspectCore.Injector;

namespace AspectCore.APM.MethodProfiler
{
    public class MethodProfilingInterceptor : AbstractInterceptorAttribute
    {
        public override bool AllowMultiple => false;
        public override int Order { get => -9999; set => throw new NotSupportedException(); }

        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            var methodProfilers = context.ServiceProvider.ResolveMany<IProfiler<MethodProfilingContext>>();
            if (!methodProfilers.Any())
            {
                await context.Invoke(next);
                return;
            }
            var methodProfilingOptions = context.ServiceProvider.ResolveRequired<IOptionAccessor<MethodProfilingOptions>>().Value;
            var samplerFactory = context.ServiceProvider.ResolveRequired<ISamplerFactory>();
            var sampler = samplerFactory.CreateSampler(methodProfilingOptions.SamplingRate.GetValueOrDefault(1));
            if (!sampler.ShouldSample())
            {
                await context.Invoke(next);
                return;
            }
            var stopwatch = Stopwatch.StartNew();
            await context.Invoke(next);
            stopwatch.Stop();
            var μs = StopwatchUtils.GetElapsedMicroseconds(stopwatch);
            var methodProfilingContext = new MethodProfilingContext
            {
                ElapsedMicroseconds = μs,
                ImplementationType = context.ImplementationMethod.DeclaringType.GetReflector().FullDisplayName,
                MethodName = context.ServiceMethod.Name,
                Namespace = context.ServiceMethod.DeclaringType.Namespace,
                ServiceType = context.ServiceMethod.DeclaringType.GetReflector().FullDisplayName
            };
            foreach (var profiler in methodProfilers)
                await profiler.Invoke(methodProfilingContext);
        }
    }
}