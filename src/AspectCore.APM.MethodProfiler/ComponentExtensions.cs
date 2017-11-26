using System;
using AspectCore.APM.Core;
using AspectCore.APM.Profiler;
using AspectCore.Configuration;
using AspectCore.Injector;

namespace AspectCore.APM.MethodProfiler
{
    public static class ComponentExtensions
    {
        public static ComponentOptions AddMethodProfiler(this ComponentOptions apmComponent, params AspectPredicate[] predicates)
        {
            if (apmComponent == null)
            {
                throw new ArgumentNullException(nameof(apmComponent));
            }
            return AddMethodProfiler(apmComponent, null, predicates);
        }

        public static ComponentOptions AddMethodProfiler(this ComponentOptions apmComponent, Action<MethodProfilingOptions> configure, params AspectPredicate[] predicates)
        {
            if (apmComponent == null)
            {
                throw new ArgumentNullException(nameof(apmComponent));
            }
            var options = new MethodProfilingOptions();
            configure?.Invoke(options);
            apmComponent.Services.AddInstance<IOptionAccessor<MethodProfilingOptions>>(options);
            apmComponent.Services.AddType<IProfiler<MethodProfilingContext>, MethodProfiler>(Lifetime.Singleton);
            apmComponent.Services.Configuration.Interceptors.AddTyped<MethodProfilingInterceptor>(predicates);
            return apmComponent;
        }
    }
}