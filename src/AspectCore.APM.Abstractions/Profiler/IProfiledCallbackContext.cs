namespace AspectCore.APM.Profiler
{
    public interface IProfiledCallbackContext
    {
        IProfiledContext ProfiledContext { get; }
    }
}