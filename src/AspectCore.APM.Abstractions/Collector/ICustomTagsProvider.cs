using AspectCore.DynamicProxy;

namespace AspectCore.APM.Collector
{
    [NonAspect]
    public interface ICustomTagsProvider
    {
        TagCollection GetTags();
    }
}