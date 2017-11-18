using System;
using System.Collections;
using System.Collections.Generic;

namespace AspectCore.APM.Collector
{
    public class Payload : IPayload
    {
        private readonly IEnumerable<IPoint> _pointes;

        public Payload(IEnumerable<IPoint> points)
        {
            _pointes = points ?? throw new ArgumentNullException(nameof(points));
        }

        public Payload(params IPoint[] points)
            : this((IEnumerable<IPoint>)points)
        {
        }

        public IEnumerator<IPoint> GetEnumerator()
        {
            return _pointes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}