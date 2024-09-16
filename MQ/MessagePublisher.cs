using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace MQ
{
    /// <summary>
    /// 消息发布者
    /// </summary>
    public class MessagePublisher:RabbitMqBase
    {


        /// <summary>
        /// 交换机配置
        /// </summary>
        private readonly ExchangeConfiguration _exchangeConfiguration;

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

        public MessagePublisher(ConnectionManager connectionManager, ExchangeConfiguration exchangeConfiguration) : base(connectionManager)
        {
            _exchangeConfiguration = exchangeConfiguration;
            initial();
        }

        /// <summary>
        /// 初始化发布者
        /// </summary>
        private void initial()
        {
            _channel.ExchangeDeclare(_exchangeConfiguration.ExchangeName, _exchangeConfiguration.ExchangeType);
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="message">待发送的消息</param>
        public void Publish(string message)
        {
            _channel.BasicPublish(exchange: _exchangeConfiguration.ExchangeName,
                                 routingKey: _exchangeConfiguration.RoutingKey,
                                 basicProperties: null,
                                 body: Encoding.UTF8.GetBytes(message));
        }
    }
}
