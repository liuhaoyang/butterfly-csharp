namespace AspectCore.APM.Profiler
{
    public interface IProfilingCallbackContext
    {
        IProfilingContext ProfilingContext { get; }
    }
}