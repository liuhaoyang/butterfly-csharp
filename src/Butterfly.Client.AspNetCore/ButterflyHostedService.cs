using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Butterfly.Client.AspNetCore
{
    public class ButterflyHostedService : IHostedService
    {
        private readonly IButterflyCollector _collector;

        public ButterflyHostedService(IButterflyCollector collector)
        {
            _collector = collector;
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            return _collector.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _collector.StopAsync(cancellationToken);
        }
    }
}