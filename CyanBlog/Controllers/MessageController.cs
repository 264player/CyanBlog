using System.Security.Cryptography;
using System.Text;
using CyanBlog.DbAccess.Context;
using CyanBlog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CyanBlog.Controllers
{
    /// <summary>
    /// 留言控制器
    /// </summary>
    public class MessageController : Controller
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
        public MessageController(ILogger<MessageController> logger, CyanBlogDbContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }

        /// <summary>
        /// 首页，留言列表
        /// </summary>
        /// <returns>首页页面</returns>
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            List<Message> messages = await _context.Message.OrderByDescending(m => m.MessageId).Include(m=>m.User).ToListAsync();
            ViewBag.MessageList = messages;
            return View();
        }


        /// <summary>
        /// 浏览管理列表，需要认证
        /// </summary>
        /// <returns>管理留言列表界面</returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> ViewList()
        {
            List<Message> messages = await _context.Message.OrderByDescending(m => m.MessageId).Include(u => u.User).Select(m =>new Message(){
                MessageId = m.MessageId,
                Content = m.Content,
                UserId = m.UserId,
                CreateTime = m.CreateTime,
                ManagerId = m.ManagerId,
                User = m.User,
            }) .ToListAsync();
            return View(messages);
        }


        /// <summary>
        /// 留言详情
        /// </summary>
        /// <param name="id">留言id</param>
        /// <returns>留言详情页面</returns>
        [HttpGet]
        public ActionResult Details(uint id)
        {
            Message? message = _context.Message.Find(id);
            if(message == null)
                return NotFound();
            return View(message);
        }

        /// <summary>
        /// 创建新留言,异步保存
        /// </summary>
        /// <param name="message">表单提交上来的留言信息</param>
        /// <returns>正确访问并提交就返回到留言首页，非正常提交就返回到NotFound</returns>
        // POST: MessageController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create( Message message)
        {
            User? exitUser = await _context.User.FirstOrDefaultAsync(u=>message.User.Email.Equals(u.Email));
            if (exitUser != null)// 用户已存在
            {
                if (!string.IsNullOrEmpty(message.User.NickName))// 用户有新昵称
                {
                    exitUser.NickName = message.User.NickName;
                    exitUser.UpdateTime = DateTime.Now;
                }
                message.User = exitUser;
            }
            else
            {
                if (string.IsNullOrEmpty(message.User.NickName))
                    message.User.NickName = "不愿意透露姓名的游客";
            }

            message.User.Password = MD5.HashData(Encoding.UTF8.GetBytes($"{message.User.UserId}-{message.User.Email}")).ToString() ?? "Cyanmoon";
            //_logger.LogInformation($"\n{GetUserIp()}参与了评论。\n用户编号为{message.User.UserId}");
            _context.Message.Add(message);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// 编辑留言内容页面
        /// </summary>
        /// <param name="id">留言id</param>
        /// <returns>返回留言编辑界面</returns>
        [HttpGet]
        [Authorize]
        public ActionResult EditView(uint id)
        {
            Message? message = _context.Message.Find(id);
            if (message == null)
                return NotFound();
            return View("Edit",message);
        }

        /// <summary>
        /// 编辑页面提交入口
        /// </summary>
        /// <param name="message">被编辑的留言</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(Message message)
        {
            User? exitUser = _context.User.Find(message.UserId);
            if(exitUser != null)
                message.User = exitUser;
            _context.Message.Update(message);
            _context.SaveChanges();
            return RedirectToAction("ViewList");
        }

        /// <summary>
        /// 删除留言页面
        /// </summary>
        /// <param name="id">留言id</param>
        /// <returns>删除留言界面的id</returns>
        [HttpGet]
        [Authorize]
        public ActionResult DeleteView(uint id)
        {
            Message? message = _context.Message.Find(id);
            if (message == null)
                return NotFound();
            return View("Delete",message);
        }

        /// <summary>
        /// 删除提交页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Delete(uint id)
        {
            Message? message = _context.Message.Find(id);
            if (message == null)
                return View("ViewList");
            _context.Message.Remove(message);
            _context.SaveChanges();
            return RedirectToAction("ViewList");
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
