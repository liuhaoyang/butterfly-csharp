using AspectCore.APM.Common;

namespace AspectCore.APM.LineProtocolCollector
{
    public class LineProtocolClientOptions : IOptionAccessor<LineProtocolClientOptions>
    {
        public string Server { get; set; }

        public string Database { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public int? Interval { get; set; }

        public int? BlockCapacity { get; set; }

        public LineProtocolClientOptions Value => this;
    }
}