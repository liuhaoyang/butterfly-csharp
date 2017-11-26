using System;
using System.Threading;
using System.Threading.Tasks;
using AspectCore.APM.Core;
using Microsoft.Extensions.Hosting;

namespace AspectCore.APM.AspNetCore
{
    public class ComponentHostedService : IHostedService
    {
        private readonly IComponentLifetime _componentLifetime;

        public ComponentHostedService(IComponentLifetime componentLifetime)
        {
            _componentLifetime = componentLifetime ?? throw new ArgumentNullException(nameof(componentLifetime));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_componentLifetime.Start());
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _componentLifetime.Stop();
            return Task.FromResult(0);
        }
    }
}