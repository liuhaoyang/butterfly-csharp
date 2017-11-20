using System.Threading.Tasks;
using AspectCore.DynamicProxy;

namespace AspectCore.APM.Profiler
{
    [NonAspect]
    public interface IProfiler<TProfilingContext> where TProfilingContext : class, IProfilingContext
    {
        Task Invoke(TProfilingContext profilingContext);
    }
}