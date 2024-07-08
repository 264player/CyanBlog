using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyanBlog.Models
{
    /// <summary>
    /// 留言信息
    /// </summary>
    public class Message
    {
        /// <summary>
        /// 留言ID
        /// </summary>
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Display(Name = "留言ID")]
        public uint MessageId {  get; set; }

        /// <summary>
        /// 留言内容
        /// </summary>
        [Required(ErrorMessage ="这就是无字天书吗")]
        [StringLength(2000)]
        [Display(Name = "留言内容")]
        public string Content {  get; set; }

        /// <summary>
        /// 留言时间
        /// </summary>
        [Required]
        [Display(Name ="创建时间")]
        public DateTime CreateTime {  get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Required]
        public uint UserId {  get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        [ForeignKey("UserId")]
        public User User {  get; set; }

        /// <summary>
        /// 审核员ID
        /// </summary>
        [Required]
        public uint ManagerId {  get; set; }

        /// <summary>
        /// 空构造函数
        /// </summary>
        public Message()
        {
            MessageId = 0;
            Content = string.Empty;
            CreateTime = DateTime.Now;
            UserId = 0;
            User = new User();
            ManagerId = 0;
        }

        /// <summary>
        /// 带有完全参数的构造函数
        /// </summary>
        /// <param name="messageId">留言ID</param>
        /// <param name="content">留言内容</param>
        /// <param name="createTime">创建时间</param>
        /// <param name="userId">用户ID</param>
        /// <param name="user">用户</param>
        /// <param name="managerId">管理员ID</param>
        public Message(uint messageId, string content, DateTime createTime, uint userId, User user, uint managerId)
        {
            MessageId = messageId;
            Content = content;
            CreateTime = createTime;
            UserId = userId;
            User = user;
            ManagerId = managerId;
        }
    }
}
