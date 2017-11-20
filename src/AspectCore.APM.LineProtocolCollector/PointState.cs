using System;
using InfluxDB.LineProtocol.Payload;

namespace AspectCore.APM.LineProtocolCollector
{
    internal class PointState
    {
        public DateTime UtcTimeStamp { get; }

        public LineProtocolPoint LineProtocolPoint { get; }

        public PointStatus Status { get; set; }

        internal PointState(LineProtocolPoint lineProtocolPoint)
        {
            UtcTimeStamp = DateTime.UtcNow;
            LineProtocolPoint = lineProtocolPoint;
        }
    }

    internal enum PointStatus
    {
        Untreated,
        Sending,
        Sended
    }
}