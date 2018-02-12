namespace Butterfly.Client
{
    public class ButterflyOptions
    {
        public string Service { get; set; }

        public string CollectorUrl { get; set; }

        public string ServiceIdentity { get; set; }

        public int BoundedCapacity { get; set; }

        public int ConsumerCount { get; set; }

        public int FlushInterval { get; set; }
    }
}