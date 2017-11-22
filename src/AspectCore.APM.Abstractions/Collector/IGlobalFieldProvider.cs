using AspectCore.DynamicProxy;

namespace AspectCore.APM.Collector
{
    [NonAspect]
    public interface IGlobalFieldProvider
    {
        FieldCollection GetGlobalFields();
    }
}
