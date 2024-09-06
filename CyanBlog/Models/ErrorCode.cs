namespace CyanBlog.Models
{
    /// <summary>
    /// 错误代码类，包含错误码和错误信息
    /// </summary>
    public class ErrorCode
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public int Code {  get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 默认构造方法
        /// code = 0 , message = ""
        /// </summary>
        public ErrorCode()
        {
            Code = 0;
            Message = string.Empty;
        }

        /// <summary>
        /// 带有全部参数的构造方法
        /// </summary>
        /// <param name="code">错误代码</param>
        /// <param name="message">错误信息</param>
        public ErrorCode(int code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}
