using System;
using System.Collections.Generic;
using System.Text;

namespace AspectCore.APM.Common
{
    public interface IOptionAccessor<TOption>
    {
        TOption Value { get; set; }
    }
}