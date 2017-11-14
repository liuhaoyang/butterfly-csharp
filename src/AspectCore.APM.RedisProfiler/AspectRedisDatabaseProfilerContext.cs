using System.Threading;
#if NET45
using System.Runtime.Remoting.Messaging;
#endif

namespace AspectCore.APM.RedisProfiler
{
    public sealed class AspectRedisDatabaseProfilerContext
    {
#if NET45
        private const string ContextKey = "Redis_Database_Profiler_Context";
        public static object Context
        {
            get
            {
                return CallContext.LogicalGetData(ContextKey);
            }
            set
            {
                CallContext.LogicalSetData(ContextKey, value);
            }
        }

#else
        private static readonly AsyncLocal<object> _context = new AsyncLocal<object>();

        public static object Context
        {
            get
            {
                return _context.Value;
            }
            set
            {
                _context.Value = value;
            }
        }
#endif
    }
}