using System;
using System.Diagnostics;
using System.Threading;
using AspectCore.APM.Core;

namespace AspectCore.APM.Collector
{
    public class ManualPoint : IDisposable
    {
        private readonly ICollector _collector;
        private readonly FieldCollection _fields;
        private readonly TagCollection _tags;
        private readonly string _measurement;
        private readonly DateTime? _utcTimestamp;
        private int _isPush;
        private long _currentTimestamp;

        internal ManualPoint(ICollector collector, string measurement, FieldCollection fields, TagCollection tags, DateTime? utcTimestamp)
        {
            _collector = collector ?? throw new ArgumentNullException(nameof(collector));
            _tags = tags;
            _fields = fields;
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
                _fields["elapsed_microseconds"] = StopwatchUtils.GetElapsedMicroseconds(_currentTimestamp);
                _collector.Push(new Payload(new Point(_measurement, _fields, _tags, _utcTimestamp.HasValue ? _utcTimestamp : DateTime.UtcNow)));
            }
        }

        public ManualPoint AddField(string key, int value)
        {
            _fields.Add(key, value);
            return this;
        }

        public ManualPoint AddField(string key, long value)
        {
            _fields.Add(key, value);
            return this;
        }

        public ManualPoint AddField(string key, float value)
        {
            _fields.Add(key, value);
            return this;
        }

        public ManualPoint AddField(string key, double value)
        {
            _fields.Add(key, value);
            return this;
        }

        public ManualPoint AddTag(string key, string value)
        {
            _tags.Add(key, value);
            return this;
        }
    }
}