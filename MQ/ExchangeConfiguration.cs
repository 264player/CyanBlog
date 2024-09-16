using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ
{
    /// <summary>
    /// 交换机配置
    /// </summary>
    public class ExchangeConfiguration
    {
        /// <summary>
        /// 交换机名称
        /// </summary>
        public string ExchangeName {  get; set; }
        /// <summary>
        /// 交换机关键字
        /// </summary>
        public string RoutingKey {  get; set; }
        /// <summary>
        /// 交换机类型
        /// </summary>
        public string ExchangeType { get; set; }

        public ExchangeConfiguration() { }

    }
}
