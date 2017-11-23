using System;
using System.Threading.Tasks;
using AspectCore.APM.Collector;
using AspectCore.APM.Core;
using AspectCore.APM.Profiler;

namespace AspectCore.APM.HttpProfiler
{
    public class HttpProfiler : IProfiler<HttpProfilingContext>
    {
        private readonly ICollector _collector;
        private readonly IGlobalTagReader _tagReader;

        public HttpProfiler(ICollector collector, IGlobalTagReader tagReader)
        {
            _collector = collector ?? throw new ArgumentNullException(nameof(collector));
            _tagReader = tagReader ?? throw new ArgumentNullException(nameof(tagReader));
        }

        public Task Invoke(HttpProfilingContext profilingContext)
        {
            var httpProfilingFields = new FieldCollection();
            var httProfilingTags = new TagCollection();
            _tagReader.Read(httProfilingTags);
            httpProfilingFields[HttpProfilingConstants.Elapsed] = profilingContext.Elapsed;
            httProfilingTags[HttpProfilingConstants.HttpHost] = profilingContext.HttpHost;
            httProfilingTags[HttpProfilingConstants.HttpMethod] = profilingContext.HttpMethod;
            httProfilingTags[HttpProfilingConstants.HttpPath] = profilingContext.HttpPath;
            httProfilingTags[HttpProfilingConstants.HttpPort] = profilingContext.HttpPort;
            httProfilingTags[HttpProfilingConstants.HttpProtocol] = profilingContext.HttpProtocol;
            httProfilingTags[HttpProfilingConstants.HttpScheme] = profilingContext.HttpScheme;
            httProfilingTags[HttpProfilingConstants.StatusCode] = profilingContext.StatusCode;
            httProfilingTags[HttpProfilingConstants.IdentityAuthenticationType] = profilingContext.IdentityAuthenticationType;
            httProfilingTags[HttpProfilingConstants.IdentityName] = profilingContext.IdentityName;
            httProfilingTags[HttpProfilingConstants.RequestContentType] = profilingContext.RequestContentType;
            httProfilingTags[HttpProfilingConstants.ResponseContentType] = profilingContext.ResponseContentType;
            var point = new Point(profilingContext.ProfilerName, httpProfilingFields, httProfilingTags);
            return Task.FromResult(_collector.Push(new Payload(new Point[] { point })));
        }
    }
}