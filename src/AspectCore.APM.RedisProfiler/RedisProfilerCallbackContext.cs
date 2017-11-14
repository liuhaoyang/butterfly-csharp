using System;
using System.Collections.Generic;
using System.Text;

namespace AspectCore.APM.RedisProfiler
{
    public sealed class RedisProfilerCallbackContext
    {
        public IEnumerable<RedisProfiledCommand> ProfiledCommands { get; }

        internal RedisProfilerCallbackContext(RedisProfilerCallbackHandlerContext redisProfilerCallbackHandlerContext)
        {
            ProfiledCommands = redisProfilerCallbackHandlerContext?.ProfiledCommands ?? throw new ArgumentNullException("redisProfilerCallbackHandlerContext.ProfiledCommands");
        }
    }
}
