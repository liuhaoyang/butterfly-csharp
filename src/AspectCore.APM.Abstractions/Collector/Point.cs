using System;

namespace AspectCore.APM.Collector
{
    public class Point : IPoint
    {
        private readonly FieldCollection _fields;
        private readonly TagCollection _tags;

        public Point(string name, FieldCollection fields, TagCollection tags, DateTime? utcTimestamp = null)
        {
            _fields = fields;
            _tags = tags;
            Name = name;
            Timestamp = utcTimestamp.HasValue ? utcTimestamp.Value : DateTime.UtcNow;
        }

        public string Name { get; }

        public DateTime Timestamp { get; }

        public FieldCollection GetFields()
        {
            return _fields;
        }

        public TagCollection GetTags()
        {
            return _tags;
        }
    }
}