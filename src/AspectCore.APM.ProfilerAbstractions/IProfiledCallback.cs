using System.Threading.Tasks;
using AspectCore.DynamicProxy;

namespace AspectCore.APM.ProfilerAbstractions
{
    [NonAspect]
    public interface IProfiledCallback<TProfiledCallbackContext> where TProfiledCallbackContext : class, IProfiledCallbackContext
    {
        Task Invoke(TProfiledCallbackContext callbackContext);
    }
}