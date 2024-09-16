using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ
{
    /// <summary>
    /// 消息队列的配置类
    /// </summary>
    public class QueueConfiguration
    {
        /// <summary>
        /// 队列名
        /// </summary>
        public string QueueName { get; set; }
        

        /// <summary>
        /// 带有全部参数的构造函数
        /// </summary>
        /// <param name="queueName">队列名</param>
        public QueueConfiguration(string queueName)
        {
            QueueName = queueName;

        }
    }
}
