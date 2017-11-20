using AspectCore.APM.Common;

namespace AspectCore.APM.ApplicationProfiler
{
    public class ApplicationProfilingOptions : IOptionAccessor<ApplicationProfilingOptions>
    {
        public int? Interval { get; set; }

        public ApplicationProfilingOptions Value => this;
    }
}