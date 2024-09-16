using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace MQ
{
    /// <summary>
    /// 消息消费者
    /// </summary>
    public class MessageConsumer:RabbitMqBase
    {
        /// <summary>
        /// 交换机配置信息
        /// </summary>
        private readonly ExchangeConfiguration _exchangeConfiguration;

        /// <summary>
        /// 队列配置信息
        /// </summary>
        private readonly QueueConfiguration _queueConfiguration;

        /// <summary>
        /// 发送消息的通道
        /// </summary>
        private IModel? _channel;

        /// <summary>
        /// 获取Channel
        /// </summary>
        public IModel Channel
        {
            get
            {
                if (_channel == null)
                    _channel = base.GetChannel();
                return _channel;
            }
        }
        public MessageConsumer(ConnectionManager connectionManager,
            QueueConfiguration queueConfiguration,
            ExchangeConfiguration exchangeConfiguration):base(connectionManager)
        {
            _exchangeConfiguration = exchangeConfiguration;
            _queueConfiguration = queueConfiguration;
        }


        private void initial()
        {
            Channel.QueueDeclare(queue: _queueConfiguration.QueueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            Channel.QueueBind(queue: _queueConfiguration.QueueName,
                exchange: _exchangeConfiguration.ExchangeName,
                routingKey: _exchangeConfiguration.RoutingKey);
        }

        public void Start(Action<string> onMessageReceived)
        {
            var consumer = new EventingBasicConsumer(Channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var routingKey = ea.RoutingKey;
                Console.WriteLine($" [x] Received '{routingKey}':'{message}'");
            };
            Channel.BasicConsume(queue: _queueConfiguration.QueueName,
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
}
