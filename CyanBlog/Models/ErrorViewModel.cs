namespace CyanBlog.Models
{
    /// <summary>
    /// ����ģ��
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// ����id
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// ����id�Ƿ�Ϊ��
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}