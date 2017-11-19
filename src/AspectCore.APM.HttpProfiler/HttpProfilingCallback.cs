using System;
using System.Threading.Tasks;
using AspectCore.APM.Collector;
using AspectCore.APM.Common;
using AspectCore.APM.Profiler;

namespace AspectCore.APM.HttpProfiler
{
    public class HttpProfilingCallback : IProfilingCallback<HttpProfilingCallbackContext>
    {
        private readonly ICollector _collector;
        private readonly ApplicationOptions _apmOptions;

        public HttpProfilingCallback(ICollector collector, IOptionAccessor<ApplicationOptions> optionAccessor)
        {
            _collector = collector ?? throw new ArgumentNullException(nameof(collector));
            _apmOptions = optionAccessor.Value;
        }

        public Task Invoke(HttpProfilingCallbackContext callbackContext)
        {
            var httpProfilingFields = new FieldCollection();
            var httProfilingTags = new TagCollection();
            httpProfilingFields.Add(HttpProfilingConstants.Elapsed, callbackContext.Elapsed);
            httProfilingTags.Add(ProfilingConstants.ApplicationName, _apmOptions.ApplicationName);
            httProfilingTags.Add(ProfilingConstants.Environment, _apmOptions.Environment);
            httProfilingTags.Add(HttpProfilingConstants.HttpHost, callbackContext.HttpHost);
            httProfilingTags.Add(HttpProfilingConstants.HttpMethod, callbackContext.HttpMethod);
            httProfilingTags.Add(HttpProfilingConstants.HttpPath, callbackContext.HttpPath);
            httProfilingTags.Add(HttpProfilingConstants.HttpPort, callbackContext.HttpPort);
            httProfilingTags.Add(HttpProfilingConstants.HttpProtocol, callbackContext.HttpProtocol);
            httProfilingTags.Add(HttpProfilingConstants.HttpScheme, callbackContext.HttpScheme);
            httProfilingTags.Add(HttpProfilingConstants.StatusCode, callbackContext.StatusCode);
            httProfilingTags.Add(HttpProfilingConstants.IdentityAuthenticationType, callbackContext.IdentityAuthenticationType);
            httProfilingTags.Add(HttpProfilingConstants.IdentityName, callbackContext.IdentityName);
            httProfilingTags.Add(HttpProfilingConstants.RequestContentType, callbackContext.RequestContentType);
            httProfilingTags.Add(HttpProfilingConstants.ResponseContentType, callbackContext.ResponseContentType);
            var point = new Point(callbackContext.ProfilingContext.ProfilerName, httpProfilingFields, httProfilingTags);
            return Task.FromResult(_collector.Push(new Payload(new Point[] { point })));
        }
    }
}