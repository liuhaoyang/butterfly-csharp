using System;
using System.Diagnostics;
using System.Threading;

namespace AspectCore.APM.Collector
{
    public class CustomPoint : IDisposable
    {
        private readonly ICollector _collector;
        private readonly FieldCollection _fields;
        private readonly TagCollection _tags;
        private readonly string _measurement;
        private readonly DateTime? _utcTimestamp;
        private int _isPush;
        private long _currentTimestamp;

        internal CustomPoint(ICollector collector, string measurement, DateTime? utcTimestamp)
        {
            _collector = collector ?? throw new ArgumentNullException(nameof(collector));
            _tags = new TagCollection();
            _fields = new FieldCollection();
            _measurement = measurement ?? throw new ArgumentNullException(nameof(measurement));
            _utcTimestamp = utcTimestamp;
            _isPush = 0;
            _currentTimestamp = Stopwatch.GetTimestamp();
        }

        public void Dispose()
        {
            Push();
        }

        public void Push()
        {
            if (Interlocked.CompareExchange(ref _isPush, 1, 0) == 0)
            {
                _fields["elapsed_milliseconds"] = ((Stopwatch.GetTimestamp() - _currentTimestamp) * 1000) / (float)Stopwatch.Frequency;
                _collector.Push(new Payload(new Point(_measurement, _fields, _tags, _utcTimestamp.HasValue ? _utcTimestamp : DateTime.UtcNow)));
            }
        }

        public CustomPoint AddField(string key, int value)
        {
            _fields.Add(key, value);
            return this;
        }

        public CustomPoint AddField(string key, long value)
        {
            _fields.Add(key, value);
            return this;
        }

        public CustomPoint AddField(string key, float value)
        {
            _fields.Add(key, value);
            return this;
        }

        public CustomPoint AddField(string key, double value)
        {
            _fields.Add(key, value);
            return this;
        }

        public CustomPoint AddTag(string key, string value)
        {
            _tags.Add(key, value);
            return this;
        }
    }
}