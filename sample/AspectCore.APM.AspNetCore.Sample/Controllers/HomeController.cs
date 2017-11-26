using System.Diagnostics;
using AspectCore.APM.AspNetCore.Sample.Models;
using AspectCore.APM.AspNetCore.Sample.Services;
using AspectCore.APM.RedisProfiler;
using Microsoft.AspNetCore.Mvc;

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

        public IActionResult Contact([FromServices]IContactService service)
        {
            ViewData["Message"] = service.GetMessage();

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
