using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CyanBlog.DbAccess.Context;
using CyanBlog.Models;
using Microsoft.AspNetCore.Authorization;

namespace CyanBlog.Controllers
{
    /// <summary>
    /// 友链控制器
    /// </summary>
    public class FriendsController : Controller
    {
        /// <summary>
        /// 数据库上下文
        /// </summary>
        private readonly CyanBlogDbContext _context;

        /// <summary>
        /// 日志记录器
        /// </summary>
        private ILogger<FriendsController> _logger;

        /// <summary>
        /// 带有数据库上下文和日志记录器的构造方法
        /// </summary>
        /// <param name="context">数据库上下文</param>
        /// <param name="logger">日志记录器</param>
        public FriendsController(ILogger<FriendsController> logger, CyanBlogDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// 友链首页,需要查询所有友链然后通过viewbag传递给视图
        /// ViewBag.FriendList
        /// 列表按照id升序排列
        /// </summary>
        /// <returns>友链首页</returns>
        public async Task<IActionResult> Index()
        {
            List<Friend> friends = await _GetFriendListAsync(false);
            ViewBag.FriendList = friends;
            return View();
        }

        /// <summary>
        /// 友链详情
        /// </summary>
        /// <param name="id">友链id</param>
        /// <returns></returns>
        public async Task<IActionResult> Details(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }else
            {
                Friend? friend = await _GetFriendAsync((uint)id);
                if (friend == null)
                {
                    return NotFound();
                }
                return View(friend);
            }
        }

        /// <summary>
        /// 创建新的友链，只绑定标题和url
        /// </summary>
        /// <param name="friend"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Url")] Friend friend)
        {

            friend.CreateTime = DateTime.Now;
            _context.Add(friend);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 编辑界面
        /// </summary>
        /// <param name="id">编辑友链id</param>
        /// <returns></returns>
        [Authorize(Roles = "ROOT")]
        public async Task<IActionResult> EditView(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                Friend? friend = await _GetFriendAsync((uint)id);
                if (friend == null)
                {
                    return NotFound();
                }
                return View("Edit", friend);
            }   
        }

        /// <summary>
        /// 编辑友链界面
        /// </summary>
        /// <param name="id"></param>
        /// <param name="friend"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "ROOT")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(uint id, [Bind("FriendId,Title,Url,PictureUrl,CreateTime")] Friend friend)
        {
            if (id != friend.FriendId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(friend);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FriendExists(friend.FriendId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ViewList));
            }
            return View(friend);
        }

        /// <summary>
        /// 删除友链界面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "ROOT")]
        public async Task<IActionResult> DeleteView(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                Friend? friend = await _GetFriendAsync((uint)id);
                if (friend == null)
                {
                    return NotFound();
                }
                return View("Delete", friend);
            }
        }
        
        /// <summary>
        /// 删除友链
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ROOT")]
        public async Task<IActionResult> DeleteConfirmed(uint id)
        {
            if (_context.Friend == null)
            {
                return Problem("Entity set is null.");
            }
            var friend = await _context.Friend.FindAsync(id);
            if (friend != null)
            {
                _context.Friend.Remove(friend);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ViewList));
        }

        /// <summary>
        /// 寻找友链是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool FriendExists(uint id)
        {
          return (_context.Friend?.Any(e => e.FriendId == id)).GetValueOrDefault();
        }

        /// <summary>
        /// 友链的管理界面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "ROOT")]
        public async Task<ActionResult> ViewList()
        {
            List<Friend> friends = await _GetFriendListAsync(true);
            return View(friends);
        }

        /// <summary>
        /// 获取友链列表，不追踪实体，提高性能
        /// </summary>
        /// <param name="isDes">是否按照主键降序排列</param>
        /// <returns></returns>
        private async Task<List<Friend>> _GetFriendListAsync(bool isDes)
        {
            if(isDes)
                return await _context.Friend.AsNoTracking().OrderByDescending(friend => friend.FriendId).ToListAsync();
            else
                return await _context.Friend.AsNoTracking().OrderBy(friend => friend.FriendId).ToListAsync();
        }

        /// <summary>
        /// 根据主键获取对应实体，如果没有查询到对应实体则返回空
        /// </summary>
        /// <param name="id">待查询的友链主键</param>
        /// <returns>查询到则返回对应友链，否则返回空</returns>
        private async Task<Friend?> _GetFriendAsync(uint id)
        {
            return await _context.Friend.FindAsync(id);
        }
    }
}