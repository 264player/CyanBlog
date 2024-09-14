using CyanBlog.Controllers;
using CyanBlog.DbAccess.Context;
using CyanBlog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyanBlogTest
{
    /// <summary>
    /// 测试友链控制器
    /// </summary>
    [TestFixture]
    public class FriendsControllerTest
    {
        private CyanBlogDbContext _context;
        private FriendsController _controller;
        private ILogger<FriendsController> _logger;

        [SetUp]
        public void SetUp()
        {
            // 使用 InMemory 数据库配置 DbContextOptions
            var options = new DbContextOptionsBuilder<CyanBlogDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new CyanBlogDbContext(options);

            _logger = new LoggerFactory().CreateLogger<FriendsController>();
            _controller = new FriendsController(_logger, _context);
            _context.Friend.RemoveRange(_context.Friend);
            _context.Friend.AddRange(
                new Friend() { FriendId = 1,Title = "FriendChain1", Url = "http://blog.top",CreateTime = DateTime.Now},
                new Friend() { FriendId = 2, Title = "FriendChain2", Url = "http://blog.top", CreateTime = DateTime.Now }
            );
            _context.SaveChanges();
        }


        /// <summary>
        /// 测试Index的模型是否正确
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Index_FriendViewTest()
        {
            var result = await _controller.Index();

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult); // 检查是否返回了视图

            var friendList = viewResult.ViewData["FriendList"] as List<Friend>;
            Assert.IsNotNull(friendList);
            Assert.AreEqual(2, friendList.Count);
            Assert.AreEqual("FriendChain1", friendList[0].Title);
        }

        /// <summary>
        /// 测试ViewList方法，测试模型是否正确
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task ListView_ReturnView()
        {
            var result = await _controller.ViewList();

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult); // 检查是否返回了视图

            var model = viewResult.Model as List<Friend>;
            Assert.IsNotNull(model);
            Assert.AreEqual(2, model.Count);
            Assert.AreEqual("FriendChain2", model[0].Title);
        }

        /// <summary>
        /// friend_details的模型测试，是否能返回正确的模型
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Details_FriendModelTest()
        {
            uint friendId = 1;
            var result = await _controller.Details(friendId);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult); // 检查是否返回了视图

            var friend1 = viewResult.Model as Friend;
            Assert.IsNotNull(friend1);
            Assert.AreEqual("FriendChain1", friend1.Title);
        }

        /// <summary>
        /// 测试视图deleteview的模型是否查找正常
        /// </summary>
        [Test]
        public async Task Delete_ViewTest()
        {
            uint friendId = 1;
            var result = await _controller.DeleteView(id: friendId);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult); // 检查是否返回了视图

            var message1 = viewResult.Model as Friend;
            Assert.IsNotNull(message1);
            Assert.AreEqual("FriendChain1", message1.Title);
        }

        /// <summary>
        /// 测试视图deleteview的模型是否查找正常
        /// </summary>
        [Test]
        public async Task Edit_ViewTest()
        {
            uint friendId = 1;
            var result = await _controller.EditView(id: friendId);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult); // 检查是否返回了视图

            var friend1 = viewResult.Model as Friend;
            Assert.IsNotNull(friend1);
            Assert.AreEqual("FriendChain1", friend1.Title);
        }

        /// <summary>
        /// 测试能否正确删除Friend
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Delete_MessageTest()
        {
            uint friendId = 1;
            await _controller.DeleteConfirmed(id: friendId);
            Friend? message = _context.Friend.Find(friendId);
            Assert.IsNull(message);
        }

        /// <summary>
        /// 测试创建新的Friend，并在数据库检验是否创建成功;
        /// </summary>
        [Test]
        public async Task Create_FriendResultTest()
        {
            Friend friend = new Friend() { FriendId = 3,Title = "friendchain3", Url = "http://mail.top" };
            await _controller.Create(friend);

            Friend? result = _context.Friend.Find(friend.FriendId);
            Assert.IsNotNull(result);
            Assert.AreEqual(friend.Title, result.Title);
        }

        /// <summary>
        /// 测试修改新的Fried，并在数据库检验是否创建成功
        /// </summary>
        [Test]
        public async Task Edit_FriendResultTest()
        {
            uint friendId = 1;
            Friend? friend = _context.Friend.Find(friendId);
            friend.Title = "UpdatefriendChain1!";
            await _controller.Edit(friendId,friend);
            Friend? result = _context.Friend.Find(friend.FriendId);
            Assert.IsNotNull(result);
            Assert.AreEqual(friend.Title, result.Title);
        }
    }
}
