using System;

namespace AspectCore.APM.Collector
{
    public static class CollectorExtensions
    {
        public static CustomPoint Point(this ICollector collector, string measurement, DateTime? utcTimestamp)
        {
            if (collector == null)
            {
                throw new ArgumentNullException(nameof(collector));
            }
            if (string.IsNullOrEmpty(measurement))
            {
                throw new ArgumentNullException(nameof(measurement));
            }
            return new CustomPoint(collector, measurement, utcTimestamp);
        }
    }
}
