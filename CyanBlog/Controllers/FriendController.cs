using CyanBlog.DbAccess.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CyanBlog.Controllers
{

    /// <summary>
    /// 友链控制器
    /// </summary>
    public class FriendController : Controller
    {
        /// <summary>
        /// 日志
        /// </summary>
        private ILogger<FriendController> _logger;

        /// <summary>
        /// CyanBlog数据库上下文
        /// </summary>
        private CyanBlogDbContext _dbContext;

        /// <summary>
        /// 拥有全部参数的构造方法
        /// </summary>
        /// <param name="logger">日志</param>
        /// <param name="dbContext">数据库上下文</param>
        public FriendController(ILogger<FriendController> logger, CyanBlogDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }



        // GET: FriendController
        public ActionResult Index()
        {
            return View();
        }

        // GET: FriendController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FriendController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FriendController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FriendController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FriendController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FriendController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FriendController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
