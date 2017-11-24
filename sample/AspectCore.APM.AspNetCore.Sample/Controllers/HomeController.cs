using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspectCore.APM.AspNetCore.Sample.Models;
using System.Threading;
using AspectCore.APM.Collector;
using AspectCore.APM.RedisProfiler;

namespace AspectCore.APM.AspNetCore.Sample.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About([FromServices]IConnectionMultiplexerProvider connectionMultiplexerProvider)
        {
            ViewData["Message"] = connectionMultiplexerProvider.ConnectionMultiplexer.GetDatabase().StringGet("message");

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
