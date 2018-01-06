using Microsoft.Extensions.Options;

namespace Butterfly.Client.AspNetCore
{
    public class ButterflyDispatcher :Client.ButterflyDispatcher
    {
        public ButterflyDispatcher(IOptions<ButterflyOptions> options) 
            : base(options.Value.BoundedCapacity, options.Value.ConsumerCount)
        {
        }
    }
}