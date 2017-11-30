using System;
using System.Threading;
using System.Threading.Tasks;
using AspectCore.APM.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace AspectCore.APM.AspNetCore
{
    public class ComponentHostedService : IHostedService
    {
        private readonly IComponentLifetime _componentLifetime;
        private readonly ApplicationOptions _applicationOptions;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ComponentHostedService(IComponentLifetime componentLifetime, IHostingEnvironment hostingEnvironment, IOptionAccessor<ApplicationOptions> optionAccessor)
        {
            _componentLifetime = componentLifetime ?? throw new ArgumentNullException(nameof(componentLifetime));
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _applicationOptions = optionAccessor.Value;
            if (string.IsNullOrEmpty(_applicationOptions.ApplicationName))
                _applicationOptions.ApplicationName = _hostingEnvironment.ApplicationName;
            if (string.IsNullOrEmpty(_applicationOptions.Environment))
                _applicationOptions.Environment = _hostingEnvironment.EnvironmentName;
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