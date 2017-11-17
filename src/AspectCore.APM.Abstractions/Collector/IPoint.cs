using System;
using AspectCore.DynamicProxy;

namespace AspectCore.APM.Collector
{
    [NonAspect]
    public interface IPoint
    {
        TagCollection GetTags();

        FieldCollection GetFields();

        string Name { get; }

        DateTime Timestamp { get; }
    }
}