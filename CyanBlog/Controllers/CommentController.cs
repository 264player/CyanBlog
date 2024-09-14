using CyanBlog.DbAccess.Context;
using CyanBlog.Models;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;

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
        private ILogger<CommentController> _logger;

        /// <summary>
        /// CyanBlog数据库上下文
        /// </summary>
        private CyanBlogDbContext _context;

        /// <summary>
        /// 带有完全参数的控制器构造方法
        /// </summary>
        /// <param name="logger">日志输出器</param>
        /// <param name="dbContext">cyanblog数据库上下文</param>
        public CommentController(ILogger<CommentController> logger, CyanBlogDbContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }

        /// <summary>
        /// 查看评论详情
        /// </summary>
        /// <param name="id">评论ID</param>
        /// <returns>对应ID的评论</returns>
        [HttpGet]
        public async Task<ActionResult> Details(uint id)
        {
            Comment? comment = await _context.Comment.FindAsync(id);
            if(comment == null)
                return NotFound();
            else
                return View(comment);
        }

        /// <summary>
        /// 创建新留言,异步保存
        /// </summary>
        /// <param name="comment">表单提交上来的评论信息</param>
        /// <returns>正确访问并提交就返回到留言首页，非正常提交就返回到NotFound</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Comment comment)
        {
            
            User? exitUser = await _context.User.FirstOrDefaultAsync(u => comment.User.Email.Equals(u.Email));
            
            if (exitUser != null)// 用户已存在
            {
                if (!comment.User.NickName.IsNullOrEmpty())// 用户有新昵称
                {
                    exitUser.NickName = comment.User.NickName;
                    exitUser.UpdateTime = DateTime.Now;
                }
                comment.User = exitUser;
            }
            else
            {
                if (comment.User.NickName.IsNullOrEmpty())
                    comment.User.NickName = "不愿意透露姓名的游客";
            }

            comment.User.Password = MD5.HashData(Encoding.UTF8.GetBytes($"{comment.User.UserId}-{comment.User.Email}")).ToString()??"Cyanmoon";

                Blog? exitBlog = await _context.Blog.FirstOrDefaultAsync(b => comment.BlogID == b.BlogID);
            if (exitBlog != null)
                comment.FatherBlog = exitBlog;
            //_logger.LogInformation($"\n{GetUserIp()}参与了评论。\n用户编号为{comment.User.UserId}");
            _context.Comment.Add(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Blog", new { id= $"{comment.BlogID}" });
        }
        
        /// <summary>
        /// 删除评论页面
        /// </summary>
        /// <param name="id">待删除的评论id</param>
        /// <returns>展示待删除的评论的页面</returns>
        [HttpGet]
        public async Task<ActionResult> Delete(uint id)
        {
            Comment? comment = await _context.Comment.FindAsync(id);
            return View(comment);
        }


        /// <summary>
        /// 管理员方法
        /// 删除指定评论
        /// </summary>
        /// <param name="id">评论id</param>
        /// <returns>管理页面</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteComment(uint id)
        {
            Comment? comment = await _context.Comment.FindAsync(id);
            if (comment != null) {
                _context.Comment.Remove(comment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("ViewList");
        }

        /// <summary>
        /// 管理员管理评论界面
        /// </summary>
        /// <returns>评论管理界面</returns>
        [Authorize]
        public async Task<ActionResult> ViewList()
        {
            return View(await _context.Comment.OrderByDescending(c=>c.CommentId).ToListAsync());
        }

        /// <summary>
        /// 获取访问服务器的ip地址，如果查询ip错误，则返回kongip；
        /// </summary>
        /// <returns>用户ip地址或者空ip</returns>
        private string GetUserIp()
        {
            return HttpContext.Connection.RemoteIpAddress != null ? HttpContext.Connection.RemoteIpAddress.ToString() : "空IP";
        }
    }
}
