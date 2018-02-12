using Microsoft.Extensions.Options;

namespace Butterfly.Client.AspNetCore
{
    public class ButterflyOptions : Client.ButterflyOptions, IOptions<ButterflyOptions>
    {
        public ButterflyOptions Value { get; }
    }
}