using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Butterfly.OpenTracing;
using Microsoft.AspNetCore.Mvc;

namespace Butterfly.Client.Sample.Frontend.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public string Get([FromServices] HttpClient httpClient, [FromServices] IServiceTracer tracer)
        {

            httpClient.GetAsync("http://localhost:5002/api/values").GetAwaiter().GetResult();

            httpClient.GetAsync("https://www.baidu.com").GetAwaiter().GetResult();

            httpClient.GetAsync("https://www.cnblogs.com").GetAwaiter().GetResult();

            return string.Empty;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
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