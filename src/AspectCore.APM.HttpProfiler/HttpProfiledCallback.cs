using System;
using System.Threading.Tasks;
using AspectCore.APM.Collector;
using AspectCore.APM.Common;
using AspectCore.APM.Profiler;

namespace AspectCore.APM.HttpProfiler
{
    public class HttpProfiledCallback : IProfiledCallback<HttpProfiledCallbackContext>
    {
        private readonly ICollector _collector;
        private readonly ApplicationOptions _apmOptions;

        public HttpProfiledCallback(ICollector collector, IOptionAccessor<ApplicationOptions> optionAccessor)
        {
            _collector = collector ?? throw new ArgumentNullException(nameof(collector));
            _apmOptions = optionAccessor.Value;
        }

        public Task Invoke(HttpProfiledCallbackContext callbackContext)
        {
            var httpProfiledFields = new FieldCollection();
            var httpProfiledTags = new TagCollection();
            httpProfiledFields.Add(HttpProfiledConstants.Elapsed, callbackContext.Elapsed);
            httpProfiledTags.Add(ProfiledConstants.ApplicationName, _apmOptions.ApplicationName);
            httpProfiledTags.Add(ProfiledConstants.Environment, _apmOptions.Environment);
            httpProfiledTags.Add(ProfiledConstants.Host, _apmOptions.Host);
            httpProfiledTags.Add(HttpProfiledConstants.HttpHost, callbackContext.HttpHost);
            httpProfiledTags.Add(HttpProfiledConstants.HttpMethod, callbackContext.HttpMethod);
            httpProfiledTags.Add(HttpProfiledConstants.HttpPath, callbackContext.HttpPath);
            httpProfiledTags.Add(HttpProfiledConstants.HttpPort, callbackContext.HttpPort);
            httpProfiledTags.Add(HttpProfiledConstants.HttpProtocol, callbackContext.HttpProtocol);
            httpProfiledTags.Add(HttpProfiledConstants.HttpScheme, callbackContext.HttpScheme);
            httpProfiledTags.Add(HttpProfiledConstants.StatusCode, callbackContext.StatusCode);
            httpProfiledTags.Add(HttpProfiledConstants.IdentityAuthenticationType, callbackContext.IdentityAuthenticationType);
            httpProfiledTags.Add(HttpProfiledConstants.IdentityName, callbackContext.IdentityName);
            httpProfiledTags.Add(HttpProfiledConstants.RequestContentType, callbackContext.RequestContentType);
            httpProfiledTags.Add(HttpProfiledConstants.ResponseContentType, callbackContext.ResponseContentType);
            var point = new Point(callbackContext.ProfiledContext.ProfilerName, httpProfiledFields, httpProfiledTags);
            return Task.FromResult(_collector.Push(new Payload(new Point[] { point })));
        }
    }
}