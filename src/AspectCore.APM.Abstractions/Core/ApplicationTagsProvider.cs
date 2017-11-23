using AspectCore.APM.Collector;
using AspectCore.APM.Profiler;

namespace AspectCore.APM.Core
{
    public class ApplicationTagsProvider : IGlobalTagProvider
    {
        private readonly ApplicationOptions _applicationOptions;

        public ApplicationTagsProvider(IOptionAccessor<ApplicationOptions> optionAccessor)
        {
            _applicationOptions = optionAccessor.Value;
        }

        public TagCollection GetGlobalTags()
        {
            var tags = new TagCollection();
            tags.Add(ProfilingConstants.ApplicationName, _applicationOptions.ApplicationName);
            tags.Add(ProfilingConstants.Environment, _applicationOptions.Environment);
            tags.Add(ProfilingConstants.Host, _applicationOptions.Host);
            return tags;
        }
    }
}
