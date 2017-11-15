using AspectCore.DynamicProxy;

namespace AspectCore.APM.Collector
{
    [NonAspect]
    public interface IPayloadClientProvider
    {
        IPayloadClient GetPayloadClient();
    }
}
