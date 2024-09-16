using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ
{
    /// <summary>
    /// 消息队列连接管理类
    /// </summary>
    public class ConnectionManager:IDisposable
    {
        /// <summary>
        /// 连接工厂
        /// </summary>
        private readonly ConnectionFactory _factory;
        /// <summary>
        /// 连接
        /// </summary>
        private IConnection _connection;

        /// <summary>
        /// 带有主机名的构造函数
        /// </summary>
        /// <param name="hostName">主机名</param>
        public ConnectionManager(string hostName)
        {
            _factory = new ConnectionFactory() { HostName = hostName };
        }

        /// <summary>
        /// 获取连接
        /// </summary>
        /// <returns>返回新的连接</returns>
        public IConnection GetConnection()
        {
            if (_connection == null || !_connection.IsOpen)
            {
                _connection = _factory.CreateConnection();
            }
            return _connection;
        }

        /// <summary>
        /// 释放连接
        /// </summary>
        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
