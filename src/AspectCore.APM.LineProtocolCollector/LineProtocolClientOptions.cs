using AspectCore.APM.Common;

namespace AspectCore.APM.LineProtocolCollector
{
    public class LineProtocolClientOptions : IOptionAccessor<LineProtocolClientOptions>
    {
        public string ServerAddress { get; set; }

        public string Database { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public LineProtocolClientOptions Value => this;
    }
}