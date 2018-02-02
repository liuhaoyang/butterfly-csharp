using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Butterfly.Client.Tracing;

namespace Butterfly.Client.Sample.Backend.Services
{
    public interface IValuesService
    {
        [Trace]
        string[] GetValues();
    }
}
