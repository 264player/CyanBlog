using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ
{
    public class RabbitMqBase
    {
        /// <summary>
        /// 连接管理
        /// </summary>
        private ConnectionManager _connectionManager;

        /// <summary>
        /// 使用连接管理类初始化消息队列基类
        /// </summary>
        /// <param name="connectionManager">消息队列连接管理</param>
        public RabbitMqBase(ConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        /// <summary>
        /// 获取一个Channel
        /// </summary>
        /// <returns>Channel实例</returns>
        protected IModel GetChannel()
        {
            return _connectionManager.GetConnection().CreateModel();
        }
    }
}
