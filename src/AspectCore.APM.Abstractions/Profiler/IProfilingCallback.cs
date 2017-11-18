using System.Threading.Tasks;
using AspectCore.DynamicProxy;

namespace AspectCore.APM.Profiler
{
    [NonAspect]
    public interface IProfilingCallback<TProfilingCallbackContext> where TProfilingCallbackContext : class, IProfilingCallbackContext
    {
        Task Invoke(TProfilingCallbackContext callbackContext);
    }
}