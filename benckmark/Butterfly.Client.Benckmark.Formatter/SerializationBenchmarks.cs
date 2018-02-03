using System;
using System.Collections.Generic;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;
using Butterfly.Client.Benckmark.Formatter.Mock;
using Butterfly.Client.Tracing;
using Butterfly.OpenTracing;
using MessagePack;
using Newtonsoft.Json;

namespace Butterfly.Client.Benckmark.Formatter
{
    [MemoryDiagnoser]
    [AllStatisticsColumn]
    public class SerializationBenchmarks
    {
        private readonly CollectionSpanRecorder recorder = new CollectionSpanRecorder();

        public SerializationBenchmarks()
        {
            Initialization();
        }

        private void Initialization()
        {
            ITracer tracer = new Tracer(recorder);
            IServiceTracer serviceTracer = new ServiceTracer(tracer, "benckmark", "debug", "Butterfly.Client.Benckmark.Formatter");
            for (var i = 0; i < 250; i++)
            {
                using (var span = serviceTracer.Start("parent"))
                {
                    span.Log(LogField.CreateNew().ServerReceive());
                    span.Tags
                     .Server().Component("AspNetCore")
                     .HttpMethod("method")
                     .HttpUrl("url")
                     .HttpHost("host")
                     .HttpPath("path")
                     .HttpStatusCode(200)
                     .PeerAddress("ip")
                     .PeerPort(8080);
                    span.Log(LogField.CreateNew().ServerSend());

                    using (var child = serviceTracer.StartChild("child"))
                    {
                    }
                }
            }
        }

        [Benchmark]
        public void JsonFormatter()
        {
            JsonConvert.SerializeObject(recorder.GetSpans());
        }

        [Benchmark]
        public void MessagePackFormatter()
        {
            MessagePackSerializer.Serialize(recorder.GetSpans());
        }
    }
}