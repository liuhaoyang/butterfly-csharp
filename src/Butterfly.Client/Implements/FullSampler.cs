using Butterfly.OpenTracing;

namespace Butterfly.Client
{
    public class FullSampler : ISampler
    {
        public bool ShouldSample()
        {
            return true;
        }
    }
}