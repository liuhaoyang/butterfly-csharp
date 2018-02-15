using Microsoft.Extensions.Options;

namespace Butterfly.Client.Console
{
    public class ButterflySenderProvider : IButterflySenderProvider
    {
        private readonly ButterflyOptions _options;

        public ButterflySenderProvider(IOptions<ButterflyOptions> options)
        {
            _options = options.Value;
        }

        public IButterflySender GetSender()
        {
            return new HttpButterflySender(_options.CollectorUrl);
        }
    }
}