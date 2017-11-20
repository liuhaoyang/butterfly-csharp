using System;
using System.Collections.Generic;
using System.Threading;
using AspectCore.APM.Common;
using AspectCore.APM.Profiler;

namespace AspectCore.APM.Collector
{
    public class CollectorLifetime : ICollectorLifetime
    {
        private readonly IInternalLogger _logger;
        private readonly IPayloadDispatcher _payloadDispatcher;
        private readonly IEnumerable<IProfilerSetup> _profilerSetups;
        private int _status = 1; // 0. started 1. stopped

        public CollectorLifetime(IPayloadDispatcher payloadDispatcher, IEnumerable<IProfilerSetup> profilerSetups, IInternalLogger logger = null)
        {
            _payloadDispatcher = payloadDispatcher ?? throw new ArgumentNullException(nameof(payloadDispatcher));
            _profilerSetups = profilerSetups ?? throw new ArgumentNullException(nameof(profilerSetups));
            _logger = logger;
        }

        public bool Start()
        {
            if (Interlocked.CompareExchange(ref _status, 0, 1) == 1)
            {
                _payloadDispatcher.Start();
                foreach (var profilerSetup in _profilerSetups)
                    profilerSetup.Start();
                _logger?.LogInformation($"AspectCore APM collector started.");
                return true;
            }

            return false;
        }

        public void Stop()
        {
            if (Interlocked.CompareExchange(ref _status, 1, 0) == 0)
            {
                foreach (var profilerSetup in _profilerSetups)
                    profilerSetup.Stop();
                _payloadDispatcher.Stop();
                _logger?.LogInformation("AspectCore APM collector stopped.");
            }
        }
    }
}