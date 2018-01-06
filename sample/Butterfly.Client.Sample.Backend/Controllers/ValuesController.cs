using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Butterfly.OpenTracing;
using Microsoft.AspNetCore.Mvc;

namespace Butterfly.Client.Sample.Backend.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get([FromServices] IServiceTracer tracer)
        {
            using (var span = tracer.StartChild("redis GET"))
            {
                span.Tags.Service("Redis");
                span.Tags.Component("StackExcnange.Redis");
                Thread.Sleep(new Random().Next(20, 135));
            }
            return new string[] {"value1", "value2"};
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            throw new Exception("test");
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}