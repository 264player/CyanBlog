using CyanBlog.DbAccess.Context;
using CyanBlog.Models;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CyanBlog.Controllers
{
    /// <summary>
    /// 评论控制器
    /// </summary>
    public class ClassifyController : Controller
    {
        /// <summary>
        /// 日志输出器
        /// </summary>
        private ILogger<ClassifyController> _logger;

        /// <summary>
        /// CyanBlog数据库上下文
        /// </summary>
        private CyanBlogDbContext _context;

        /// <summary>
        /// 带有完全参数的控制器构造方法
        /// </summary>
        /// <param name="logger">日志输出器</param>
        /// <param name="dbContext">cyanblog数据库上下文</param>
        public ClassifyController(ILogger<ClassifyController> logger, CyanBlogDbContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }

        /// <summary>
        /// 首页，分类列表
        /// </summary>
        /// <returns>首页页面</returns>
        // GET: MessageController
        public async Task<ActionResult> Index()
        {
            var ip = GetUserIp();
            _logger.LogInformation($"\n{ip}访问Classify-Index");
            List<Classify> classifies = await _context.Classify.ToListAsync();
            ViewData["BlogGroup"] = GetBlogGroup(classifies);
            return View(classifies);
        }

        private List<List<Blog>> GetBlogGroup(List<Classify> classifies)
        {
            List<List<Blog>> blogGroup = new List<List<Blog>>();
            List<Blog> blogList = _context.Blog.Select(b => new Blog
            {
                BlogID = b.BlogID,
                Title = b.Title,
                Description = b.Description,
                ClassId = b.ClassId
            }).ToList();
            foreach (Classify classify in classifies)
            {
                List<Blog> newGroup = new List<Blog>();
                for (int i = 0; i < blogList.Count; i++)
                {
                    if (blogList[i].ClassId == classify.ClassId)
                    {
                        newGroup.Add(blogList[i]);
                        blogList.RemoveAt(i);
                        i--;
                    }
                }
                blogGroup.Add(newGroup);
            }
            return blogGroup;
        }

        // GET: MessageController/Details/5
        public ActionResult Details(int id)
        {
            return NotFound();
        }

        // GET: MessageController/Create
        public ActionResult Create()
        {
            return NotFound();
        }

 
        // POST: MessageController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Classify classify)
        {
            return NotFound();
        }

        // GET: MessageController/Edit/5
        public ActionResult Edit(int id)
        {
            return NotFound();
        }

        // POST: MessageController/Edit/5
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
                return NotFound();
            }
        }

        // GET: MessageController/Delete/5
        public ActionResult Delete(int id)
        {
            return NotFound();
        }

        // POST: MessageController/Delete/5
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
                return NotFound();
            }
        }

        /// <summary>
        /// 获取访问服务器的ip地址，如果查询ip错误，则返回kongip；
        /// </summary>
        /// <returns>用户ip地址或者空ip</returns>
        private string GetUserIp()
        {
            return HttpContext.Connection.RemoteIpAddress != null ? HttpContext.Connection.RemoteIpAddress.ToString() : "空IP";
        }


        private string LogModelState()
        {
            StringBuilder summary = new StringBuilder();
            foreach (var key in ModelState.Keys)
            {
                var value = ModelState[key];
                foreach (var info in value.Errors)
                {
                    summary.Append(key + ":" + info);
                }
            }
            return summary.ToString();
        }
    }
}
