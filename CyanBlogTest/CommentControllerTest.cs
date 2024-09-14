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
    /// 测试评论控制器
    /// </summary>
    [TestFixture]
    public class CommentControllerTest
    {
        private CyanBlogDbContext _context;
        private CommentController _controller;
        private ILogger<CommentController> _logger;

        [SetUp]
        public void SetUp()
        {
            // 使用 InMemory 数据库配置 DbContextOptions
            var options = new DbContextOptionsBuilder<CyanBlogDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new CyanBlogDbContext(options);

            _logger = new LoggerFactory().CreateLogger<CommentController>();
            _controller = new CommentController(_logger, _context);
            _context.Comment.RemoveRange(_context.Comment);
            _context.Classify.RemoveRange(_context.Classify);
            _context.Blog.RemoveRange(_context.Blog);
            _context.User.RemoveRange(_context.User);
            _context.SaveChanges();
            Classify defaultclassify = new Classify() { Name = "default" };
            Blog fatherBlog1 = new Blog()
            {
                BlogID = 1,
                Title = "blogTitle1",
                Description = "blogDescription1",
                Content = "blogContent1",
                CreateTime = DateTime.Now,
                Classify = defaultclassify,
            },
            fatherBlog2 = new Blog()
            {
                BlogID = 2,
                Title = "blogTitle2",
                Description = "blogDescription2",
                Content = "blogContent2",
                CreateTime = DateTime.Now,
                Classify = defaultclassify,
            };
            _context.Blog.AddRange(fatherBlog1,fatherBlog2);
            //fatherBlog1 = null;
            //fatherBlog2 = null;
            _context.Comment.AddRange(
                new Comment() {
                    CommentId = 1,
                    BlogID = 1,
                    FatherBlog = fatherBlog1,
                    Content = "testContent1",
                    UserId = 1,
                },
                new Comment()
                {
                    CommentId = 2,
                    BlogID = 2,
                    FatherBlog = fatherBlog2,
                    Content = "testContent2",
                    UserId = 2,
                }
            ) ;
            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Comment.RemoveRange(_context.Comment);
            _context.Classify.RemoveRange(_context.Classify);
            _context.Blog.RemoveRange(_context.Blog);
            _context.User.RemoveRange(_context.User);
            _context.SaveChanges();
        }


        /// <summary>
        /// 测试ViewList的模型是否正确
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task ViewList_CommentViewTest()
        {
            var result = await _controller.ViewList();

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult); // 检查是否返回了视图

            var commentList = viewResult.Model as List<Comment>;
            Assert.IsNotNull(commentList);
            Assert.AreEqual(2, commentList.Count);
            Assert.AreEqual("testContent2", commentList[0].Content);
        }

        /// <summary>
        /// friend_details的模型测试，是否能返回正确的模型
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Details_CommentModelTest()
        {
            uint commentId = 1;
            var result = await _controller.Details(commentId);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult); // 检查是否返回了视图

            var comment1 = viewResult.Model as Comment;
            Assert.IsNotNull(comment1);
            Assert.AreEqual("testContent1", comment1.Content);
        }

        /// <summary>
        /// 测试视图deleteview的模型是否查找正常
        /// </summary>
        [Test]
        public async Task Delete_ViewTest()
        {
            uint commentId = 1;
            var result = await _controller.Delete(id: commentId);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult); // 检查是否返回了视图

            var comment1 = viewResult.Model as Comment;
            Assert.IsNotNull(comment1);
            Assert.AreEqual("testContent1", comment1.Content);
        }

        /// <summary>
        /// 测试能否正确删除Friend
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Delete_CommentTest()
        {
            uint commentid = 1;
            await _controller.DeleteComment(id: commentid);
            Comment? comment = _context.Comment.Find(commentid);
            Assert.IsNull(comment);
        }

        /// <summary>
        /// 测试创建新的Friend，并在数据库检验是否创建成功;
        /// </summary>
        [Test]
        public async Task Create_CommentResultTest()
        {
            Comment newComment = new Comment()
            {
                CommentId = 3,
                BlogID = 23,
                Content = "testContent3",
                UserId = 3,
            };
            await _controller.Create(newComment);

            Comment? result = _context.Comment.Find(newComment.CommentId);
            Assert.IsNotNull(result);
            Assert.AreEqual(newComment.Content, result.Content);
        }
    }
}
