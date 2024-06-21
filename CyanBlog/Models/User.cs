using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyanBlog.Models
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Display(Name ="用户ID")]
        public uint UserId {  get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [Required]
        [StringLength(50)]
        [Display(Name = "昵称")]
        public string NickName {  get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        [StringLength(50)]
        [Display(Name = "用户名")]
        public string UserName {  get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        [StringLength(35)]
        [Display(Name = "密码")]
        public string Password {  get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name = "邮箱")]
        public string Email {  get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        [Required]
        public UserType Type {  get; set; }

        /// <summary>
        /// 头像路径
        /// </summary>
        [Required]
        [Url]
        public string HeadPictureUrl {  get; set; }

        /// <summary>
        /// 用户创建时间
        /// </summary>
        [Required]
        public DateTime CreateTime {  get; set; }

        /// <summary>
        /// 用户信息更新时间
        /// </summary>
        [Required]
        public DateTime UpdateTime {  get; set; }

        /// <summary>
        /// 默认的构造函数
        /// </summary>
        public User()
        {
            UserId = 0;
            NickName = string.Empty;
            UserName = string.Empty;
            Password = string.Empty;
            Email = string.Empty;
            Type = UserType.NORMAL;
            HeadPictureUrl = string.Empty;
            CreateTime = DateTime.MinValue;
            UpdateTime = DateTime.MinValue;
        }

        /// <summary>
        /// 带有完全参数的构造函数
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="nickName">昵称</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="email">邮箱</param>
        /// <param name="type">用户类型</param>
        /// <param name="headPictureUrl">头像路径</param>
        /// <param name="createTime">新建用户时间</param>
        /// <param name="updateTime">更新时间</param>
        public User(uint userId, string nickName, string userName, string password, string email, UserType type, string headPictureUrl, DateTime createTime, DateTime updateTime)
        {
            UserId = userId;
            NickName = nickName;
            UserName = userName;
            Password = password;
            Email = email;
            Type = type;
            HeadPictureUrl = headPictureUrl;
            CreateTime = createTime;
            UpdateTime = updateTime;
        }
    }
    
    /// <summary>
    /// 用户类型
    /// </summary>
    public enum UserType
    {
        /// <summary>
        /// 普通用户
        /// </summary>
        NORMAL = 0,
        /// <summary>
        /// 管理员
        /// </summary>
        ROOT = 1
    }
}
