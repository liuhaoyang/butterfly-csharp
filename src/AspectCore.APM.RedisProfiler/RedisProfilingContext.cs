using System;
using System.Collections.Generic;
using AspectCore.APM.Profiler;

namespace AspectCore.APM.RedisProfiler
{
    public sealed class RedisProfilingContext : IProfilingContext
    {
        public string ProfilerName => "redis_client";

        public IEnumerable<RedisProfilingCommand> ProfilingCommands { get; }

        internal RedisProfilingContext(IEnumerable<RedisProfilingCommand> profilingCommands)
        {
            ProfilingCommands = profilingCommands ?? throw new ArgumentNullException(nameof(profilingCommands));
        }
    }
}