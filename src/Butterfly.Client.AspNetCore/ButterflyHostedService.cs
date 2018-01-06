using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace Butterfly.Client.AspNetCore
{
    public class ButterflyHostedService : IHostedService
    {
        private readonly IButterflyCollector _collector;

        public ButterflyHostedService(IButterflyCollector collector, IEnumerable<ITracingDiagnosticListener> tracingDiagnosticListeners, DiagnosticListener diagnosticListener)
        {
            _collector = collector;
            foreach (var tracingDiagnosticListener in tracingDiagnosticListeners)
            {
                diagnosticListener.SubscribeWithAdapter(tracingDiagnosticListener);
            }
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