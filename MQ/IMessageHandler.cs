using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ
{
    /// <summary>
    /// 规定处理消息的逻辑
    /// </summary>
    public interface IMessageHandler
    {
        public Task ExecuteAsync();
    }
}
