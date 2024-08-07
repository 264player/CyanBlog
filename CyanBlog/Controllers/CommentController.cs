﻿using CyanBlog.DbAccess.Context;
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
    public class CommentController : Controller
    {
        /// <summary>
        /// 日志输出器
        /// </summary>
        private ILogger<MessageController> _logger;

        /// <summary>
        /// CyanBlog数据库上下文
        /// </summary>
        private CyanBlogDbContext _context;

        /// <summary>
        /// 带有完全参数的控制器构造方法
        /// </summary>
        /// <param name="logger">日志输出器</param>
        /// <param name="dbContext">cyanblog数据库上下文</param>
        public CommentController(ILogger<MessageController> logger, CyanBlogDbContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }

        /// <summary>
        /// 首页，留言列表
        /// </summary>
        /// <returns>首页页面</returns>
        // GET: MessageController
        public async Task<ActionResult> Index()
        {
            var ip = GetUserIp();
            _logger.LogInformation($"\n{ip}访问Message-Index");
            List<Message> messages = await _context.Message.OrderByDescending(m => m.MessageId).ToListAsync();
            ViewBag.MessageList = messages;
            return View();
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

        /// <summary>
        /// 创建新留言,异步保存
        /// </summary>
        /// <param name="comment">表单提交上来的评论信息</param>
        /// <returns>正确访问并提交就返回到留言首页，非正常提交就返回到NotFound</returns>
        // POST: MessageController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Comment comment)
        {
            User? exitUser = await _context.User.FirstOrDefaultAsync(u => comment.User.Email.Equals(u.Email));
            if (exitUser != null)
                comment.User = exitUser;
            Blog? exitBlog = await _context.Blog.FirstOrDefaultAsync(b => comment.BlogID == b.BlogID);
            if (exitBlog != null)
                comment.FatherBlog = exitBlog;
            _logger.LogInformation($"\n{GetUserIp()}访问了/Create-通过了前端验证");
            _context.Comment.Add(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Blog", new { id= $"{comment.BlogID}" });
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
