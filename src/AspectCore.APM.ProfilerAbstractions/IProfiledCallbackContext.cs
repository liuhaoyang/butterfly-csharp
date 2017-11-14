namespace AspectCore.APM.ProfilerAbstractions
{
    public interface IProfiledCallbackContext
    {
        IProfiledContext ProfiledContext { get; }
    }
}