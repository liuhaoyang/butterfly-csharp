using System;
using System.Threading.Tasks;
using AspectCore.APM.Collector;
using AspectCore.APM.Common;
using AspectCore.APM.Profiler;

namespace AspectCore.APM.ApplicationProfiler
{
    public class ApplicationGCProfiler : IProfiler<ApplicationGCProfilingContext>
    {
        private readonly ICollector _collector;
        private readonly ApplicationOptions _applicationOptions;

        public ApplicationGCProfiler(ICollector collector, IOptionAccessor<ApplicationOptions> optionAccessor)
        {
            _collector = collector ?? throw new ArgumentNullException(nameof(collector));
            _applicationOptions = optionAccessor.Value;
        }

        public Task Invoke(ApplicationGCProfilingContext profilingContext)
        {
            var profilingFields = new FieldCollection();
            var profilingTags = new TagCollection();

            profilingFields.Add(ApplicationProfilingConstants.Gen0_CollectionCount, profilingContext.Gen0_CollectionCount);
            profilingFields.Add(ApplicationProfilingConstants.Gen1_CollectionCount, profilingContext.Gen1_CollectionCount);
            profilingFields.Add(ApplicationProfilingConstants.Gen2_CollectionCount, profilingContext.Gen2_CollectionCount);
            profilingFields.Add(ApplicationProfilingConstants.TotalCollectionCount, profilingContext.TotalCollectionCount);
            profilingFields.Add(ApplicationProfilingConstants.TotalMemory, profilingContext.TotalMemory);

            profilingTags.Add(ProfilingConstants.ApplicationName, _applicationOptions.ApplicationName);
            profilingTags.Add(ProfilingConstants.Environment, _applicationOptions.Environment);
            profilingTags.Add(ApplicationProfilingConstants.GCLatencyMode, profilingContext.GCLatencyMode);
            profilingTags.Add(ApplicationProfilingConstants.GCMode, profilingContext.GCMode);

            var point = new Point(profilingContext.ProfilerName, profilingFields, profilingTags, DateTime.UtcNow);
            return Task.FromResult(_collector.Push(new Payload(point)));
        }
    }
}