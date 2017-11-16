using System;
using System.Collections.Generic;
using System.Text;

namespace AspectCore.APM.LineProtocolCollector
{
    public class LineProtocolClientOptions
    {
        public string ServerAddress { get; set; }

        public string Database { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}