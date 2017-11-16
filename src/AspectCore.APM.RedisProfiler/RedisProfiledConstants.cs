using System;
using System.Collections.Generic;
using System.Text;

namespace AspectCore.APM.RedisProfiler
{
    internal static class RedisProfiledConstants
    {
        public const string Elapsed = "executed_elapsed";

        public const string OperationCount = "operation_count";

        public const string ClientName = "client_name";

        public const string Command = "command";

        public const string Db = "db";

        public const string Server = "server";
    }
}
