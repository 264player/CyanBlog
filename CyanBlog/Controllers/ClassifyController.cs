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
        public async Task<ActionResult> Index()
        {
            List<Classify> classifies = await _context.Classify.ToListAsync();
            var blogGroup = GetBlogGroup(classifies);
            List<Classify> clist = new List<Classify>();
            List<List<Blog>> blist = new List<List<Blog>>();
            foreach(Classify classify in blogGroup.Keys)
            {
                if(blogGroup[classify].Count != 0)
                {
                    clist.Add(classify);
                    blist.Add(blogGroup[classify]);
                }
            }
            ViewBag.BlogGroup = blist;
            return View(clist);
        }

        private Dictionary<Classify, List<Blog>> GetBlogGroup(List<Classify> classifies)
        {
            Dictionary<Classify, List<Blog>> bg = new Dictionary<Classify, List<Blog>>();
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
                bg.Add(classify, newGroup);
            }
            return bg;
        }
    }
}
