using AspectCore.DynamicProxy;

namespace AspectCore.APM.Core
{
    [NonAspect]
    public interface ISampler
    {
        bool ShouldSample();
    }
}