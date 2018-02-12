using Microsoft.Extensions.Options;

namespace Butterfly.Client.AspNetCore
{
    public class ButterflyOptions : IOptions<ButterflyOptions>
    {
        public ButterflyOptions Value { get; }

        public string Service { get; set; }

        public string ServiceIdentity { get; set; }

        public string CollectorUrl { get; set; }

        public int BoundedCapacity { get; set; }

        public int ConsumerCount { get; set; }

        public int FlushInterval { get; set; }
    }
}