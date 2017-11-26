using AspectCore.APM.Core;

namespace AspectCore.APM.MethodProfiler
{
    public class MethodProfilingOptions : IOptionAccessor<MethodProfilingOptions>
    {
        public MethodProfilingOptions Value => this;

        public float? SamplingRate { get; set; }
    }
}