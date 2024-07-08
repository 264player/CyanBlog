using System.Diagnostics;
using CyanBlog.Models;
using Microsoft.AspNetCore.Mvc;

namespace CyanBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var ip = HttpContext.Connection.RemoteIpAddress!=null? HttpContext.Connection.RemoteIpAddress.ToString() : "kong";
            _logger.LogInformation($"{ip}--{this.GetType().FullName}Home/Index被请求");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}