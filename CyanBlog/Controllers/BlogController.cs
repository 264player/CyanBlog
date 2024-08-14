using CyanBlog.DbAccess.Context;
using CyanBlog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CyanBlog.Controllers
{
    /// <summary>
    /// 博客控制器
    /// </summary>
    public class BlogController : Controller
    {
        /// <summary>
        /// 日志
        /// </summary>
        private ILogger<BlogController> _logger;

        /// <summary>
        /// CyanBlog数据库上下文
        /// </summary>
        private CyanBlogDbContext _dbContext;

        /// <summary>
        /// 带有日志输出器和数据库上下文的构造方法
        /// </summary>
        /// <param name="logger">日志输出器</param>
        /// <param name="dbContext">CyanBlog数据库上下文</param>
        public BlogController(ILogger<BlogController> logger, CyanBlogDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }




        /// <summary>
        /// 博客首页
        /// </summary>
        /// <returns></returns>
        // GET: BlogController
        public async Task<ActionResult> Index()
        {
            return View(await _dbContext.Blog.OrderByDescending(blog => blog.BlogID).ToListAsync<Blog>());
        }

        /// <summary>
        /// 通过博客Id查找具体的博客,如果没有正确的路由会返回404
        /// </summary>
        /// <param name="id">博客id</param>
        /// <returns></returns>
        // GET: BlogController/Details/5
        public async Task<ActionResult> Details(uint? id)
        {
            if(id == null)
            {
                NotFound();
            }
            Blog? blog = await _dbContext.Blog.Include(b=>b.Classify).FirstAsync(b => b.BlogID == id);
            List<Comment> comments = await _dbContext.Comment.Where(comment=>comment.BlogID == id).ToListAsync();
            if (blog == null)
            {
                NotFound();
            }
            ViewData["BlogDetails"] = blog;
            ViewData["CommentList"] = comments;
            return View();
        }

        /// <summary>
        /// 新建博客页面
        /// </summary>
        /// <returns>返回新建博客页面</returns>
        // GET: BlogController/Create
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> CreateBlog()
        {
            ViewData["ClassifyList"] = await _dbContext.Classify.ToListAsync();
            return View("Create");
        }

        /// <summary>
        /// 新建博客提交页面，跳转到首页,对于博客分类，如果有新的分类就添加，没有新的分类就忽略
        /// </summary>
        /// <param name="blog">需要提交的博客</param>
        /// <returns>返回到首页</returns>
        // POST: BlogController/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Blog blog)
        {
            Classify? exitclassify =  await _dbContext.Classify.FirstOrDefaultAsync(c => c.Name.CompareTo(blog.Classify.Name)==0);
            if(exitclassify != null)
                blog.Classify = exitclassify;
            await _dbContext.Blog.AddAsync(blog);
            _logger.LogInformation($"新增了一个博客{blog.BlogID}--{blog.Title}");
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 根据博客id转移到编辑页面
        /// </summary>
        /// <param name="id">待编辑的博客id</param>
        /// <returns>编辑博客页面</returns>
        // GET: BlogController/Edit/5
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> EditBlog(uint id)
        {
            Blog? blog = _dbContext.Blog.Find(id);
            ViewData["ClassifyList"] = await _dbContext.Classify.ToListAsync();
            if (blog == null)
                return NotFound();
            return View("Update", blog);
        }

        // POST: BlogController/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Blog blog)
        {
            Classify? exitclassify =  _dbContext.Classify.FirstOrDefault(c => c.Name.CompareTo(blog.Classify.Name) == 0);
            if (exitclassify != null)
                blog.Classify = exitclassify;
            _dbContext.Blog.Update(blog);
            _dbContext.SaveChanges();
            return RedirectToAction("ViewList");
        }

        /// <summary>
        /// 根据博客id，返回删除界面
        /// </summary>
        /// <param name="id">待删除的id</param>
        /// <returns>删除博客界面</returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> DeleteBlog(uint? id)
        {
            if (id == null)
            {
                NotFound();
            }
            Blog? blog = await _dbContext.Blog.Include(b=>b.Classify).SingleAsync(b=>b.BlogID == id);
            if (blog == null)
            {
                NotFound();
            }
            return View("Delete",blog);
        }

        // POST: BlogController/Delete/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Remove(uint id)
        {
            Blog? blog = _dbContext.Blog.Find(id);   
            if(blog != null)
            {
                _dbContext.Blog.Remove(blog);
                _dbContext.SaveChanges();
            }
            return RedirectToAction("ViewList");
        }

        [Authorize]
        public async Task<ActionResult> ViewList()
        {
            return View(await _dbContext.Blog.OrderByDescending(blog => blog.BlogID).ToListAsync<Blog>());
        }

        /// <summary>
        /// 时间线页面
        /// </summary>
        /// <returns>返回时间线页面</returns>
        public async Task<ActionResult> TimeLine()
        {
            var blogList = await _dbContext.Blog.OrderByDescending(b=>b.BlogID).ToListAsync();
            return View(blogList);
        }
    }
}
