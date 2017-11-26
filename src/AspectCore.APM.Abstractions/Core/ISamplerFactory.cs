using AspectCore.DynamicProxy;

namespace AspectCore.APM.Core
{
    [NonAspect]
    public interface ISamplerFactory
    {
        ISampler CreateSampler(float samplingRate);
    }
}