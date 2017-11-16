using System;
using System.Collections.Generic;
using System.Text;

namespace AspectCore.APM.Common
{
    public class APMOptions
    {
        public string ApplicationName { get; set; }

        public string Environment { get; set; }

        public string Host { get; set; }

        public string Urls { get; set; }
    }
}