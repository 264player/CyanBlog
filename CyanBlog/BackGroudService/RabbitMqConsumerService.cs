using CyanBlog.DbAccess.Context;
using CyanBlog.MessageQueue;
using CyanBlog.Models;
using Microsoft.EntityFrameworkCore;
using MQ;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using ExchangeType = RabbitMQ.Client.ExchangeType;

namespace CyanBlog.BackGroudService
{
    public class RabbitMqConsumerService : BackgroundService
    {
        private MessageConsumer _consuumer;
        private IServiceScopeFactory _serviceScopeFactory;
        public RabbitMqConsumerService(MessageConsumer consumer, IServiceScopeFactory serviceScopeFactory)
        {
            _consuumer = consumer;
            _consuumer.ExchangeConfiguration = new ExchangeConfiguration()
            {
                ExchangeName = "blog",
                ExchangeType = ExchangeType.Direct,
                RoutingKey = "ViewCountIncreament"
            };
            _consuumer.QueueConfiguration = new QueueConfiguration("blog-ViewCountIncreament");
            _consuumer.initial();
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consuumer.Start(async message =>
            {
                var handler = JsonConvert.DeserializeObject<BlogViewCountIncreament>(message);
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    handler._dbContext = scope.ServiceProvider.GetRequiredService<CyanBlogDbContext>();
                    await handler.ExecuteAsync();
                }
            });
            return Task.CompletedTask;
        }
    }

}
