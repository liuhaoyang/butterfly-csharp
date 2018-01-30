using Butterfly.OpenTracing;

namespace Butterfly.Client.Tracing
{
    public class FullSampler : ISampler
    {
        public bool ShouldSample()
        {
            return true;
        }
    }
}