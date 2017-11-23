using AspectCore.APM.Collector;
using AspectCore.DynamicProxy;

namespace AspectCore.APM.Core
{
    [NonAspect]
    public interface IGlobalFieldReader
    {
        void Read(FieldCollection fieldCollection);
    }
}