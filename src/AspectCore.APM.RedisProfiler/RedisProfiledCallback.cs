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
                redisProfiledFields.Add(RedisProfiledConstants.Elapsed, command.Elapsed.Milliseconds);
                redisProfiledFields.Add(RedisProfiledConstants.OperationCount, command.OperationCount.ToString());
                redisProfiledTags.Add(ProfiledConstants.ApplicationName, _apmOptions.ApplicationName);
                redisProfiledTags.Add(ProfiledConstants.Environment, _apmOptions.Environment);
                redisProfiledTags.Add(ProfiledConstants.Host, _apmOptions.Host);
                redisProfiledTags.Add(RedisProfiledConstants.ClientName, command.ClientName);
                redisProfiledTags.Add(RedisProfiledConstants.Command, command.Command);
                redisProfiledTags.Add(RedisProfiledConstants.Db, command.Db.ToString());
                redisProfiledTags.Add(RedisProfiledConstants.Server, command.Server.ToString());
                points.Add(new Point(callbackContext.ProfiledContext.ProfilerName, redisProfiledFields, redisProfiledTags));
            }
            return Task.FromResult(_collector.Push(new Payload(points)));
        }
    }
}