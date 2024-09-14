using CyanBlog.Controllers;
using CyanBlog.DbAccess.Context;
using CyanBlog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CyanBlogTest
{
    /// <summary>
    /// 博客控制器测试类
    /// </summary>
    [TestFixture]
    public class BlogControllerTest
    {
        private CyanBlogDbContext _context;
        private BlogController _controller;
        private ILogger<BlogController> _logger;

        [SetUp]
        public void Setup()
        {
            // 使用 InMemory 数据库配置 DbContextOptions
            var options = new DbContextOptionsBuilder<CyanBlogDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new CyanBlogDbContext(options);

            _logger = new LoggerFactory().CreateLogger<BlogController>();
            _controller = new BlogController(_logger, _context);
            _context.Blog.RemoveRange(_context.Blog);
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
            _context.Blog.AddRange(
                fatherBlog1,fatherBlog2
            );
            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        /// <summary>
        /// 测试Index的action
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Index_ReturnsView_WithMessageList()
        {
            var result = await _controller.Index();

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult); // 检查是否返回了视图

            var blogList = viewResult.Model as List<Blog>;
            Assert.IsNotNull(blogList);
            Assert.AreEqual(2, blogList.Count);
            Assert.AreEqual("blogTitle2", blogList[0].Title);
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

            var blogList = viewResult.Model as List<Blog>;
            Assert.IsNotNull(blogList);
            Assert.AreEqual(2, blogList.Count);
            Assert.AreEqual("blogTitle2", blogList[0].Title);
        }

        /// <summary>
        /// 测试视图deleteview的模型是否查找正常
        /// </summary>
        [Test]
        public async Task Delete_ViewTest()
        {
            uint blogId = 1;
            var result = await _controller.DeleteBlog(id: blogId);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult); // 检查是否返回了视图

            var blog1 = viewResult.Model as Blog;
            Assert.IsNotNull(blog1);
            Assert.AreEqual("blogTitle1", blog1.Title);
        }

        /// <summary>
        /// 测试视图deleteview的模型是否查找正常
        /// </summary>
        [Test]
        public async Task Edit_ViewTest()
        {
            uint blogId = 1;
            var result = await _controller.EditBlog(id: blogId);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult); // 检查是否返回了视图

            var blog1 = viewResult.Model as Blog;
            Assert.IsNotNull(blog1);
            Assert.AreEqual("blogTitle1", blog1.Title);
        }

        /// <summary>
        /// 测试能否正确删除
        /// </summary>
        /// <returns></returns>
        [Test]
        public void Delete_BlogTest()
        {
            uint blogId = 1;
            _controller.Remove(id: blogId);
            Blog? blog = _context.Blog.Find(blogId);
            Assert.IsNull(blog);
        }

        /// <summary>
        /// 测试创建新的Message，并在数据库检验是否创建成功;
        /// </summary>
        [Test]
        public async Task Create_MessageResultTest()
        {
            Blog blog = new Blog()
            {
                BlogID = 4,
                Title = "blogTitle4",
                Description = "blogDescription4",
                Content = "blogContent4",
                CreateTime = DateTime.Now,
                Classify = new Classify()
                {
                    ClassId = 2,
                    Name = "Test",
                }
            };
            await _controller.Create(blog);

            Blog? result = _context.Blog.Find(blog.BlogID);
            Assert.IsNotNull(result);
            Assert.AreEqual(blog.Content, result.Content);
        }

        /// <summary>
        /// 测试修改新的Message，并在数据库检验是否创建成功
        /// </summary>
        [Test]
        public void Update_MessageResultTest()
        {
            uint blogId = 1;
            Blog? blog = _context.Blog.Find(blogId);
            blog.Title = "UpdateTitle1!";
            _controller.Edit(blog);
            Blog? result = _context.Blog.Find(blog.BlogID);
            Assert.IsNotNull(result);
            Assert.AreEqual(blog.Title, result.Title);
        }
    }
}
