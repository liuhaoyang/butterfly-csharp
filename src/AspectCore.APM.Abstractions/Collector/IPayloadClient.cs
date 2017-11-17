using System.Threading;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;

namespace AspectCore.APM.Collector
{
    [NonAspect]
    public interface IPayloadClient
    {
        Task WriteAsync(IPayload payload, CancellationToken cancellationToken = default(CancellationToken));
    }
}