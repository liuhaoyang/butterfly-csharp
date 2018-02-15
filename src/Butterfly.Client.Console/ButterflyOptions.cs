using Microsoft.Extensions.Options;

namespace Butterfly.Client.Console
{
    public class ButterflyOptions : Client.ButterflyOptions, IOptions<ButterflyOptions>
    {
        public ButterflyOptions Value { get; }
    }
}