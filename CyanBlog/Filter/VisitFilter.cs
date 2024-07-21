using Microsoft.AspNetCore.Mvc.Filters;

namespace CyanBlog.Filter
{
    /// <summary>
    /// 用于记录每一个用户访问的日志过滤器
    /// </summary>
    public class VisitFilter : IAsyncActionFilter
    {
        /// <summary>
        /// 日志器
        /// </summary>
        private readonly ILogger<VisitFilter> _logger;

        /// <summary>
        /// 带有日志的构造函数
        /// </summary>
        /// <param name="logger">日志</param>
        public VisitFilter(ILogger<VisitFilter> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 过滤器
        /// </summary>
        /// <param name="context">web上下文</param>
        /// <param name="next">下一个过滤器</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger.LogInformation($"{GetUserIp(context.HttpContext)} Visit {context.HttpContext.Request.Path}");
            var executedContext = await next();
        }
        /// <summary>
        /// 获取访问服务器的ip地址，如果查询ip错误，则返回kongip；
        /// </summary>
        /// <returns>用户ip地址或者空ip</returns>
        private string GetUserIp(HttpContext httpContext)
        {
            return httpContext.Connection.RemoteIpAddress != null ? httpContext.Connection.RemoteIpAddress.ToString() : "空IP";
        }

    }
}
