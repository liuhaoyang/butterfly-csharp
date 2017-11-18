using System;
using System.Collections.Generic;
using AspectCore.APM.Profiler;

namespace AspectCore.APM.RedisProfiler
{
    public sealed class RedisProfilingCallbackContext : IProfilingCallbackContext
    {
        public IProfilingContext ProfilingContext { get; } = new RedisProfilingContext();

        public IEnumerable<RedisProfilingCommand> ProfilingCommands { get; }

        internal RedisProfilingCallbackContext(IEnumerable<RedisProfilingCommand> profilingCommands)
        {
            ProfilingCommands = profilingCommands ?? throw new ArgumentNullException(nameof(profilingCommands));
        }
    }
}