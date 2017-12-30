using System.Threading;
using System.Threading.Tasks;

namespace Butterfly.Client
{
    public interface IButterflyCollector
    {
        Task StartAsync(CancellationToken cancellationToken);
        
        Task StopAsync(CancellationToken cancellationToken);
    }
}