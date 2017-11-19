using System;
using System.Threading.Tasks;
using AspectCore.APM.Collector;
using AspectCore.APM.Common;
using AspectCore.APM.Profiler;

namespace AspectCore.APM.ApplicationProfiler
{
    public class ApplicationGCProfilingCallback : IProfilingCallback<ApplicationGCProfilingCallbackContext>
    {
        private readonly ICollector _collector;
        private readonly ApplicationOptions _applicationOptions;

        public ApplicationGCProfilingCallback(ICollector collector, IOptionAccessor<ApplicationOptions> optionAccessor)
        {
            _collector = collector ?? throw new ArgumentNullException(nameof(collector));
            _applicationOptions = optionAccessor.Value;
        }

        public Task Invoke(ApplicationGCProfilingCallbackContext callbackContext)
        {
            var profilingFields = new FieldCollection();
            var profilingTags = new TagCollection();

            profilingFields.Add(ApplicationProfilingConstants.Gen0_CollectionCount, callbackContext.Gen0_CollectionCount);
            profilingFields.Add(ApplicationProfilingConstants.Gen1_CollectionCount, callbackContext.Gen1_CollectionCount);
            profilingFields.Add(ApplicationProfilingConstants.Gen2_CollectionCount, callbackContext.Gen2_CollectionCount);
            profilingFields.Add(ApplicationProfilingConstants.TotalCollectionCount, callbackContext.TotalCollectionCount);
            profilingFields.Add(ApplicationProfilingConstants.TotalMemory, callbackContext.TotalMemory);

            profilingTags.Add(ProfilingConstants.ApplicationName, _applicationOptions.ApplicationName);
            profilingTags.Add(ProfilingConstants.Environment, _applicationOptions.Environment);
            profilingTags.Add(ApplicationProfilingConstants.GCLatencyMode, callbackContext.GCLatencyMode);
            profilingTags.Add(ApplicationProfilingConstants.GCMode,"Server");

            var point = new Point(callbackContext.ProfilingContext.ProfilerName, profilingFields, profilingTags, DateTime.UtcNow);
            return Task.FromResult(_collector.Push(new Payload(point)));
        }
    }
}
