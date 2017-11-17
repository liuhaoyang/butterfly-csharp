using System;
using System.Collections.Generic;
using AspectCore.APM.Profiler;

namespace AspectCore.APM.RedisProfiler
{
    public sealed class RedisProfiledCallbackContext : IProfiledCallbackContext
    {
        public IProfiledContext ProfiledContext { get; } = new RedisProfiledContext();

        public IEnumerable<RedisProfiledCommand> ProfiledCommands { get; }

        internal RedisProfiledCallbackContext(IEnumerable<RedisProfiledCommand> profiledCommands)
        {
            ProfiledCommands = profiledCommands ?? throw new ArgumentNullException(nameof(profiledCommands));
        }
    }
}