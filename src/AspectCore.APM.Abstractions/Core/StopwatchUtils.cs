using System.Diagnostics;

namespace AspectCore.APM.Core
{
    public static class StopwatchUtils
    {
        /// <summary>
        /// Convert ticks to microsecond(μs)
        /// </summary>
        public static long GetElapsedMicroseconds(Stopwatch stopwatch)
        {
            return (long)((stopwatch.ElapsedTicks / (float)Stopwatch.Frequency) * 1000000);
        }

        /// <summary>
        /// Convert ticks to microsecond(μs)
        /// </summary>
        public static long GetElapsedMicroseconds(long timestamp)
        {
            return (long)(((Stopwatch.GetTimestamp() - timestamp) / (float)Stopwatch.Frequency) * 1000000);
        }
    }
}