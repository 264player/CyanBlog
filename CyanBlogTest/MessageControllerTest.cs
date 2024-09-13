using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CyanBlog.DbAccess.Context;
using CyanBlog.Controllers;
using NLog;
using Message = CyanBlog.Models.Message;
using User = CyanBlog.Models.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace CyanBlogTest
{
    /// <summary>
    /// �������Կ�������ҳ�淵�ص���Ϊ�Ƿ���ȷ
    /// </summary>
    [TestFixture]
    public class MessageControllerTests
    {
        private CyanBlogDbContext _context;
        private MessageController _controller;
        private ILogger<MessageController> _logger;

        [SetUp]
        public void Setup()
        {
            // ʹ�� InMemory ���ݿ����� DbContextOptions
            var options = new DbContextOptionsBuilder<CyanBlogDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new CyanBlogDbContext(options);

            _logger = new LoggerFactory().CreateLogger<MessageController>();
            _controller = new MessageController(_logger, _context);
            _context.Message.RemoveRange(_context.Message);
            _context.Message.AddRange(
                new Message { MessageId = 1, Content = "Test Message 1", User = new User { NickName = "User 1" } },
                new Message { MessageId = 2, Content = "Test Message 2", User = new User { NickName = "User 2" } }
            );
            _context.SaveChanges();
        }

        /// <summary>
        /// ����Index��action
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Index_ReturnsView_WithMessageList()
        {
            var result = await _controller.Index();

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult); // ����Ƿ񷵻�����ͼ

            var messageList = viewResult.ViewData["MessageList"] as List<Message>;
            Assert.IsNotNull(messageList);
            Assert.AreEqual(2, messageList.Count);
            Assert.AreEqual("Test Message 2", messageList[0].Content);
        }

        /// <summary>
        /// ����ViewList����������ģ���Ƿ���ȷ
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task ListView_ReturnView()
        {
            var result = await _controller.ViewList();

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult); // ����Ƿ񷵻�����ͼ

            var model = viewResult.Model as List<Message>;
            Assert.IsNotNull(model);
            Assert.AreEqual(2, model.Count);
            Assert.AreEqual("Test Message 2", model[0].Content);
        }

        /// <summary>
        /// message_details��ģ�Ͳ��ԣ��Ƿ��ܷ�����ȷ��ģ��
        /// </summary>
        /// <returns></returns>
        [Test]
        public void Details_MessageModelTest()
        {
            uint messageId = 1;
            var result = _controller.Details(id:messageId);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult); // ����Ƿ񷵻�����ͼ

            var message1 = viewResult.Model as Message;
            Assert.IsNotNull(message1);
            Assert.AreEqual("Test Message 1", message1.Content);
        }

        /// <summary>
        /// ������ͼdeleteview��ģ���Ƿ��������
        /// </summary>
        [Test]
        public void Delete_ViewTest()
        {
            uint messageId = 1;
            var result = _controller.DeleteView(id: messageId);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult); // ����Ƿ񷵻�����ͼ

            var message1 = viewResult.Model as Message;
            Assert.IsNotNull(message1);
            Assert.AreEqual("Test Message 1", message1.Content);
        }

        /// <summary>
        /// ������ͼdeleteview��ģ���Ƿ��������
        /// </summary>
        [Test]
        public void Edit_ViewTest()
        {
            uint messageId = 1;
            var result = _controller.EditView(id: messageId);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult); // ����Ƿ񷵻�����ͼ

            var message1 = viewResult.Model as Message;
            Assert.IsNotNull(message1);
            Assert.AreEqual("Test Message 1", message1.Content);
        }

        /// <summary>
        /// �����ܷ���ȷɾ��
        /// </summary>
        /// <returns></returns>
        [Test]
        public void Delete_MessageTest()
        {
            uint messageId = 1;
            _controller.Delete(id: messageId);
            Message? message = _context.Message.Find(messageId);
            Assert.IsNull(message);
        }

        /// <summary>
        /// ���Դ����µ�Message���������ݿ�����Ƿ񴴽��ɹ�;
        /// </summary>
        [Test]
        public async Task Create_MessageResultTest()
        {
            Message message = new Message { MessageId = 3, Content = "Test Message 3", User = new User {UserId = (uint)3, NickName = "User 3" } };
            await _controller.Create(message);

            Message? result = _context.Message.Find(message.MessageId);
            Assert.IsNotNull(result);
            Assert.AreEqual(message.Content, result.Content);
        }

        /// <summary>
        /// �����޸��µ�Message���������ݿ�����Ƿ񴴽��ɹ�
        /// </summary>
        [Test]
        public void Update_MessageResultTest()
        {
            uint messageId = 1;
            Message? message = _context.Message.Find(messageId);
            message.Content = "UpdateMessage1!";
            _controller.Edit(message);
            Message? result = _context.Message.Find(message.MessageId);
            Assert.IsNotNull(result);
            Assert.AreEqual(message.Content, result.Content);
        }
    }
}