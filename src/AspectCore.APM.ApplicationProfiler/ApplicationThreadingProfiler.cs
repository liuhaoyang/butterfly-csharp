using System;
using System.Threading.Tasks;
using AspectCore.APM.Collector;
using AspectCore.APM.Common;
using AspectCore.APM.Profiler;

namespace AspectCore.APM.ApplicationProfiler
{
    public class ApplicationThreadingProfiler : IProfiler<ApplicationThreadingProfilingContext>
    {
        private readonly ICollector _collector;
        private readonly ApplicationOptions _applicationOptions;

        public ApplicationThreadingProfiler(ICollector collector, IOptionAccessor<ApplicationOptions> optionAccessor)
        {
            _collector = collector ?? throw new ArgumentNullException(nameof(collector));
            _applicationOptions = optionAccessor.Value;
        }

        public Task Invoke(ApplicationThreadingProfilingContext profilingContext)
        {
            var profilingFields = new FieldCollection();
            var profilingTags = new TagCollection();

            profilingFields.Add(ApplicationProfilingConstants.AvailableCompletionPortThreads, profilingContext.AvailableCompletionPortThreads);
            profilingFields.Add(ApplicationProfilingConstants.AvailableWorkerThreads, profilingContext.AvailableWorkerThreads);
            profilingFields.Add(ApplicationProfilingConstants.MinCompletionPortThreads, profilingContext.MinCompletionPortThreads);
            profilingFields.Add(ApplicationProfilingConstants.MinWorkerThreads, profilingContext.MinWorkerThreads);
            profilingFields.Add(ApplicationProfilingConstants.MaxCompletionPortThreads, profilingContext.MaxCompletionPortThreads);
            profilingFields.Add(ApplicationProfilingConstants.MaxWorkerThreads, profilingContext.MaxWorkerThreads);
          
            profilingTags.Add(ProfilingConstants.ApplicationName, _applicationOptions.ApplicationName);
            profilingTags.Add(ProfilingConstants.Environment, _applicationOptions.Environment);

            var point = new Point(profilingContext.ProfilerName, profilingFields, profilingTags, DateTime.UtcNow);
            return Task.FromResult(_collector.Push(new Payload(point)));
        }
    }
}