using AspectCore.DynamicProxy;

namespace AspectCore.APM.Collector
{
    [NonAspect]
    public interface IGlobalTagProvider
    {
        TagCollection GetGlobalTags();
    }
}