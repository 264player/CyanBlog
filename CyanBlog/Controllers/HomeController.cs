using System.Diagnostics;
using CyanBlog.Models;
using Microsoft.AspNetCore.Mvc;

namespace CyanBlog.Controllers
{
    /// <summary>
    /// 默认控制器
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// 默认控制器的带日志输出的构造方法
        /// </summary>
        /// <param name="logger"></param>
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        
        /// <summary>
        /// 开发阶段的错误返回界面
        /// </summary>
        /// <returns>返回错误页面</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}