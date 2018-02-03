using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Butterfly.Client.Sample.Backend.Services
{
    public class ValuesService : IValuesService
    {
        public string[] GetValues()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
