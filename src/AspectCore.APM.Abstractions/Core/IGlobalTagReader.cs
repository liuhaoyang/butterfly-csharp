using AspectCore.APM.Collector;
using AspectCore.DynamicProxy;

namespace AspectCore.APM.Core
{
    [NonAspect]
    public interface IGlobalTagReader
    {
        void Read(TagCollection tagCollection);
    }
}
