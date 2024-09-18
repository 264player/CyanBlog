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
        private ExchangeConfiguration? _exchangeConfiguration;

        public ExchangeConfiguration? ExchangeConfiguration { 
            get
            { 
                return _exchangeConfiguration;
            }
            set
            {
                _exchangeConfiguration = value;
                initial();
            }
        }

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

        public MessagePublisher(ConnectionManager connectionManager) : base(connectionManager)
        {
        }

        /// <summary>
        /// 初始化发布者
        /// </summary>
        public void initial()
        {
            if (_exchangeConfiguration != null)
            {
                Channel.ExchangeDeclare(_exchangeConfiguration.ExchangeName, _exchangeConfiguration.ExchangeType,true);
            }
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="message">待发送的消息</param>
        public void Publish(string message)
        {
            if(_exchangeConfiguration != null)
            {
                Channel.BasicPublish(exchange: _exchangeConfiguration.ExchangeName,
                                 routingKey: _exchangeConfiguration.RoutingKey,
                                 basicProperties: null,
                                 body: Encoding.UTF8.GetBytes(message));
            }
        }
    }
}
