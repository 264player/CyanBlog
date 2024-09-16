using CyanBlog.DbAccess.Context;
using CyanBlog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Text;
using System.Threading.Channels;
using Channel = RabbitMQ.Client.IModel;

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
        private CyanBlogDbContext _context;

        /// <summary>
        /// 消息队列连接
        /// </summary>
        private Channel _channel;

        /// <summary>
        /// 带有日志输出器和数据库上下文的构造方法
        /// </summary>
        /// <param name="logger">日志输出器</param>
        /// <param name="dbContext">CyanBlog数据库上下文</param>
        /// <param name="channel">消息队列连接</param>
        public BlogController(ILogger<BlogController> logger, CyanBlogDbContext dbContext, Channel channel)
        {
            _logger = logger;
            _context = dbContext;
            _channel = channel;
        }

        /// <summary>
        ///  带有日志输出器和数据库上下文的构造方法
        /// </summary>
        /// <param name="logger">日志输出器</param>
        /// <param name="dbContext">CyanBlog数据库上下文</param>
        public BlogController(ILogger<BlogController> logger, CyanBlogDbContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }


        /// <summary>
        /// 博客首页
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            return View(await _GetBlogListAsync(true));
        }

        /// <summary>
        /// 通过博客Id查找具体的博客,如果没有正确的路由会返回404
        /// </summary>
        /// <param name="id">博客id</param>
        /// <returns></returns>
        public async Task<ActionResult> Details(uint? id)
        {
            if(id == null)
            {
                NotFound();
            }
            Blog? blog = await _context.Blog.Include(b=>b.Classify).FirstAsync(b => b.BlogID == id);
            List<Comment> comments = await _context.Comment.Where(comment=>comment.BlogID == id).Include(c=>c.User).ToListAsync();
            blogViewCountIncreament(id);
            await _context.SaveChangesAsync();
            if (blog == null)
            {
                NotFound();
            }
            ViewData["BlogDetails"] = blog;
            ViewData["CommentList"] = comments;
            return View();
        }

        /// <summary>
        /// 博客的浏览量增加
        /// </summary>
        private void blogViewCountIncreament(uint? blogID)
        {
            var message = blogID.ToString();
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "",
                                 routingKey: "viewcount_queue",
                                 mandatory: false,
                                 basicProperties: null,
                                 body: body);
        }

        /// <summary>
        /// 新建博客页面
        /// </summary>
        /// <returns>返回新建博客页面</returns>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> CreateBlog()
        {
            ViewData["ClassifyList"] = await _context.Classify.ToListAsync();
            return View("Create");
        }

        /// <summary>
        /// 新建博客提交页面，跳转到首页,对于博客分类，如果有新的分类就添加，没有新的分类就忽略
        /// </summary>
        /// <param name="blog">需要提交的博客</param>
        /// <returns>返回到首页</returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Blog blog)
        {
            Classify? exitclassify =  await _context.Classify.FirstOrDefaultAsync(c => c.Name.CompareTo(blog.Classify.Name)==0);
            if(exitclassify != null)
                blog.Classify = exitclassify;
            await _context.Blog.AddAsync(blog);
            //_logger.LogInformation($"新增了一个博客{blog.BlogID}--{blog.Title}");
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 根据博客id转移到编辑页面
        /// </summary>
        /// <param name="id">待编辑的博客id</param>
        /// <returns>编辑博客页面</returns>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> EditBlog(uint id)
        {
            Blog? blog = await _GetBlogAsync(id);
            ViewData["ClassifyList"] = await _context.Classify.ToListAsync();
            if (blog == null)
                return NotFound();
            return View("Update", blog);
        }

        /// <summary>
        /// // POST: BlogController/Edit/5
        /// 修改博客的接口
        /// </summary>
        /// <param name="blog">修改之后的博客</param>
        /// <returns>重定向到博客管理首页</returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Blog blog)
        {
            Classify? exitclassify =  _context.Classify.FirstOrDefault(c => c.Name.CompareTo(blog.Classify.Name) == 0);
            if (exitclassify != null)
                blog.Classify = exitclassify;
            blog.UpdateTime = DateTime.Now;
            _context.Blog.Update(blog);
            _context.SaveChanges();
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
            Blog? blog = await _context.Blog.Include(b=>b.Classify).SingleAsync(b=>b.BlogID == id);
            if (blog == null)
            {
                NotFound();
            }
            return View("Delete",blog);
        }

        /// <summary>
        /// 管理员删除博客的接口
        /// </summary>
        /// <param name="id">需要删除的博客的id</param>
        /// <returns>重定向到博客管理首页</returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Remove(uint id)
        {
            Blog? blog = _context.Blog.Find(id);   
            if(blog != null)
            {
                _context.Blog.Remove(blog);
                _context.SaveChanges();
            }
            return RedirectToAction("ViewList");
        }

        /// <summary>
        /// 博客管理的首页
        /// </summary>
        /// <returns>博客管理首页页面</returns>
        [Authorize]
        public async Task<ActionResult> ViewList()
        {
            return View(await _GetBlogListAsync(true));
        }

        /// <summary>
        /// 时间线页面
        /// </summary>
        /// <returns>返回时间线页面</returns>
        public async Task<ActionResult> TimeLine()
        {
            return View(await _GetBlogListAsync(true));
        }

        /// <summary>
        /// 获取全部博客，并按照指定的顺序排列，不追踪，提高性能
        /// </summary>
        /// <param name="isDes">是否按照主键降序排列</param>
        /// <returns>所有的博客列表</returns>
        private async Task<List<Blog>> _GetBlogListAsync(bool isDes)
        {
            if(isDes)
                return await _context.Blog.AsNoTracking().OrderByDescending(blog=>blog.BlogID).ToListAsync();
            else
                return await _context.Blog.AsNoTracking().OrderBy(blog => blog.BlogID).ToListAsync();
        }

        /// <summary>
        /// 获取对应主键的博客
        /// </summary>
        /// <param name="id">博客主键</param>
        /// <returns>查找成功则返回对应博客，否则返回空</returns>
        private async Task<Blog?> _GetBlogAsync(uint id)
        {
            return await _context.Blog.FindAsync(id);
        }
    }
}
