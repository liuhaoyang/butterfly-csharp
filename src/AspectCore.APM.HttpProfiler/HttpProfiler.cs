using System;
using System.Threading.Tasks;
using AspectCore.APM.Collector;
using AspectCore.APM.Common;
using AspectCore.APM.Profiler;

namespace AspectCore.APM.HttpProfiler
{
    public class HttpProfiler : IProfiler<HttpProfilingContext>
    {
        private readonly ICollector _collector;
        private readonly ApplicationOptions _apmOptions;

        public HttpProfiler(ICollector collector, IOptionAccessor<ApplicationOptions> optionAccessor)
        {
            _collector = collector ?? throw new ArgumentNullException(nameof(collector));
            _apmOptions = optionAccessor.Value;
        }

        public Task Invoke(HttpProfilingContext profilingContext)
        {
            var httpProfilingFields = new FieldCollection();
            var httProfilingTags = new TagCollection();
            httpProfilingFields.Add(HttpProfilingConstants.Elapsed, profilingContext.Elapsed);
            httProfilingTags.Add(ProfilingConstants.ApplicationName, _apmOptions.ApplicationName);
            httProfilingTags.Add(ProfilingConstants.Environment, _apmOptions.Environment);
            httProfilingTags.Add(HttpProfilingConstants.HttpHost, profilingContext.HttpHost);
            httProfilingTags.Add(HttpProfilingConstants.HttpMethod, profilingContext.HttpMethod);
            httProfilingTags.Add(HttpProfilingConstants.HttpPath, profilingContext.HttpPath);
            httProfilingTags.Add(HttpProfilingConstants.HttpPort, profilingContext.HttpPort);
            httProfilingTags.Add(HttpProfilingConstants.HttpProtocol, profilingContext.HttpProtocol);
            httProfilingTags.Add(HttpProfilingConstants.HttpScheme, profilingContext.HttpScheme);
            httProfilingTags.Add(HttpProfilingConstants.StatusCode, profilingContext.StatusCode);
            httProfilingTags.Add(HttpProfilingConstants.IdentityAuthenticationType, profilingContext.IdentityAuthenticationType);
            httProfilingTags.Add(HttpProfilingConstants.IdentityName, profilingContext.IdentityName);
            httProfilingTags.Add(HttpProfilingConstants.RequestContentType, profilingContext.RequestContentType);
            httProfilingTags.Add(HttpProfilingConstants.ResponseContentType, profilingContext.ResponseContentType);
            var point = new Point(profilingContext.ProfilerName, httpProfilingFields, httProfilingTags);
            return Task.FromResult(_collector.Push(new Payload(new Point[] { point })));
        }
    }
}