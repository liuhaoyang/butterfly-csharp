using System;
using AspectCore.APM.Core;

namespace AspectCore.APM.Collector
{
    public class ManualPointFactory : IManualPointFactory
    {
        private readonly ICollector _collector;
        private readonly IGlobalTagReader _tagReader;

        public ManualPointFactory(ICollector collector, IGlobalTagReader tagReader)
        {
            _collector = collector ?? throw new ArgumentNullException(nameof(collector));
            _tagReader = tagReader ?? throw new ArgumentNullException(nameof(tagReader));
        }

        public ManualPoint Create(string measurement, DateTime? utcTimestamp = null)
        {
            var tagCollection = new TagCollection();
            _tagReader.Read(tagCollection);
            return new ManualPoint(_collector, measurement, new FieldCollection(), tagCollection, utcTimestamp);
        }
    }
}