using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspectCore.APM.Collector;
using AspectCore.APM.Common;
using AspectCore.APM.Profiler;

namespace AspectCore.APM.RedisProfiler
{
    public class RedisProfiledCallback : IProfiledCallback<RedisProfiledCallbackContext>
    {
        private readonly ICollector _collector;
        private readonly APMOptions _apmOptions;

        public RedisProfiledCallback(ICollector collector, IOptionAccessor<APMOptions> optionAccessor)
        {
            _collector = collector ?? throw new ArgumentNullException(nameof(collector));
            _apmOptions = optionAccessor.Value;
        }

        public Task Invoke(RedisProfiledCallbackContext callbackContext)
        {
            var points = new List<Point>();
            foreach(var command in callbackContext.ProfiledCommands)
            {
                var redisProfiledFields = new FieldCollection();
                var redisProfiledTags = new TagCollection();
                redisProfiledFields.Add(nameof(command.Elapsed), command.Elapsed.Milliseconds);
                redisProfiledFields.Add(nameof(command.OperationCount), command.OperationCount.ToString());
                redisProfiledTags.Add(nameof(_apmOptions.ApplicationName), _apmOptions.ApplicationName);
                redisProfiledTags.Add(nameof(_apmOptions.Environment), _apmOptions.Environment);
                redisProfiledTags.Add(nameof(_apmOptions.Host), _apmOptions.Host);
                redisProfiledTags.Add(nameof(command.ClientName), command.ClientName);
                redisProfiledTags.Add(nameof(command.Command), command.Command);
                redisProfiledTags.Add(nameof(command.Db), command.Db.ToString());
                redisProfiledTags.Add(nameof(command.Server), command.Server.ToString());
                points.Add(new Point(callbackContext.ProfiledContext.ProfilerName, redisProfiledFields, redisProfiledTags));
            }
            return Task.FromResult(_collector.Push(new Payload(points)));
        }
    }
}