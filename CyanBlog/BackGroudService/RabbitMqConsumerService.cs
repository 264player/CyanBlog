using CyanBlog.DbAccess.Context;
using CyanBlog.Models;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace CyanBlog.BackGroudService
{
    public class RabbitMqConsumerService : BackgroundService
    {
        private readonly IModel _channel;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RabbitMqConsumerService(IModel channel, IServiceScopeFactory serviceScopeFactory)
        {
            _channel = channel;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                uint blogID = uint.Parse(message);
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var _dbContext = scope.ServiceProvider.GetRequiredService<CyanBlogDbContext>();
                    // 执行数据库的 count++ 操作
                    Blog? blog = _dbContext.Blog.Find(blogID);
                    if (blog != null)
                    {
                        blog.ViewCount++;
                        _dbContext.SaveChanges();
                    }
                    // 手动确认消息已被处理
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
            };

            _channel.BasicConsume(queue: "viewcount_queue",
                                 autoAck: false, // 手动确认
                                 consumer: consumer);

            return Task.CompletedTask;
        }
    }

}
