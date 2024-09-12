
using CyanBlog.Models;
using Microsoft.AspNetCore.Http;
using System.Web;

namespace CyanBlog.Middleware
{
    /// <summary>
    /// 处理错误代码跳转页面
    /// </summary>
    public class ErrorHandlingMiddleware : IMiddleware
    {
        /// <summary>
        /// 处理错误代码的方法
        /// </summary>
        /// <param name="context">http上下文</param>
        /// <param name="next">下一个中间件</param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // 执行下一个中间件
            await next(context);
            bool isHandle = true;
            int statusCode = context.Response.StatusCode;
            string message = "默认错误";
            // 检查响应状态码
            switch (statusCode)
            {
                case StatusCodes.Status400BadRequest:
                    message = "请求无效，服务器无法理解请求的格式。";
                    break;
                case StatusCodes.Status403Forbidden:
                    message = "服务器拒绝请求，服务器没有权限访问该资源";
                    break;
                case StatusCodes.Status404NotFound:
                    message = "请求的资源未找到，服务器无法找到所请求的URL";
                    break;
                case StatusCodes.Status405MethodNotAllowed:
                    message = "请求方法不被允许，服务器不支持请求所使用方法";
                    break;
                case StatusCodes.Status406NotAcceptable:
                    message = "请求资源的类型不接受。";
                    break;
                case StatusCodes.Status408RequestTimeout:
                    message = "请求超时，服务器在等待请求超时";
                    break;
                case StatusCodes.Status409Conflict:
                    message = "请求与服务器当前的状态冲突。";
                    break;
                case StatusCodes.Status410Gone:
                    message = "请求的资源已经被永久删除。";
                    break;
                case StatusCodes.Status413PayloadTooLarge:
                    message = "请求的负载过大，服务器无法处理。";
                    break;
                case StatusCodes.Status415UnsupportedMediaType:
                    message = "服务器拒绝处理请求，内容类型不匹配。";
                    break;
                case StatusCodes.Status429TooManyRequests:
                    message = "请求过于频繁。";
                    break;
                case StatusCodes.Status500InternalServerError:
                    message = "服务器内部错误，无法完成请求。";
                    break;
                case StatusCodes.Status502BadGateway:
                    message = "服务器不支持请求功能。";
                    break;
                case StatusCodes.Status503ServiceUnavailable:
                    message = "服务器当前无法处理请求。";
                    break;
                case StatusCodes.Status504GatewayTimeout:
                    message = "作为网关或代理的服务器未能及时从上游服务器获取请求。";
                    break;
                default:
                    isHandle = false;
                    break;
            }
            if (isHandle)
            {
                context.Response.Redirect($"/Home/ErrorCodePage?code={statusCode}&message={HttpUtility.UrlEncode(message)}");
                return;
            }   
        }
    }

}
