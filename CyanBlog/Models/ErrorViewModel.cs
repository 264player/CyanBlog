namespace CyanBlog.Models
{
    /// <summary>
    /// 报错模型
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// 请求id
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// 请求id是否不为空
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}