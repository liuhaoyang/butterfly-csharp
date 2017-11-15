using AspectCore.DynamicProxy;

namespace AspectCore.APM.Collector
{
    [NonAspect]
    public interface ITagsProvider
    {
        TagCollection GetTags();
    }
}