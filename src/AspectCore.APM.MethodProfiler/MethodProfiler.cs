using System;
using System.Threading.Tasks;
using AspectCore.APM.Collector;
using AspectCore.APM.Core;
using AspectCore.APM.Profiler;

namespace AspectCore.APM.MethodProfiler
{
    public class MethodProfiler : IProfiler<MethodProfilingContext>
    {
        private readonly ICollector _collector;
        private readonly IGlobalTagReader _tagReader;

        public MethodProfiler(ICollector collector, IGlobalTagReader tagReader)
        {
            _collector = collector ?? throw new ArgumentNullException(nameof(collector));
            _tagReader = tagReader ?? throw new ArgumentNullException(nameof(tagReader));
        }

        public Task Invoke(MethodProfilingContext profilingContext)
        {
            var profilingFields = new FieldCollection();
            var profilingTags = new TagCollection();
            _tagReader.Read(profilingTags);
            profilingFields[MethodProfilingConstants.ElapsedMicroseconds] = profilingContext.ElapsedMicroseconds;
            profilingTags[MethodProfilingConstants.ImplementationType] = profilingContext.ImplementationType;
            profilingTags[MethodProfilingConstants.MethodName] = profilingContext.MethodName;
            profilingTags[MethodProfilingConstants.Namespace] = profilingContext.Namespace;
            profilingTags[MethodProfilingConstants.ServiceType] = profilingContext.ServiceType;
            var point = new Point(profilingContext.ProfilerName, profilingFields, profilingTags);
            return Task.FromResult(_collector.Push(new Payload(point)));
        }
    }
}