using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyanBlog.Models
{
    /// <summary>
    /// 评论类
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// 留言ID
        /// </summary>
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Display(Name = "")]
        public uint CommentId { get; set; }

        /// <summary>
        /// 留言内容
        /// </summary>
        [Required]
        [StringLength(2000)]
        [Display(Name ="回复内容")]
        public string Content { get; set; }

        /// <summary>
        /// 留言时间
        /// </summary>
        [Required]
        [Display(Name ="回复时间")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 博客ID
        /// </summary>
        [Required]
        [Display(Name = "博客ID")]
        public uint BlogID { get; set; }

        /// <summary>
        /// 评论所属的博客
        /// </summary>
        [ForeignKey("BlogID")]
        public Blog FatherBlog { get; set; }
        /// <summary>
        /// 父评论ID
        /// </summary>
        public uint? FatherID {  get; set; }

        /// <summary>
        /// 父评论
        /// </summary>
        public Comment? FatherComment { get; set; }

        /// <summary>
        /// 子评论集合
        /// </summary>
        public List<Comment>? ChildComments { get; set;}

        /// <summary>
        /// 用户ID
        /// </summary>
        [Required]
        [Display(Name = "用户ID")]
        public uint UserId { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        [Required]
        [ForeignKey("UserId")]
        public User User { get; set; }

        /// <summary>
        /// 审核员ID
        /// </summary>
        [Required]
        public uint ManagerId { get; set; }

        /// <summary>
        /// 空构造函数
        /// </summary>
        public Comment()
        {
            CommentId = 0;
            Content = string.Empty;
            CreateTime = DateTime.Now;
            BlogID = 0;
            FatherBlog = new Blog();
            FatherID = 0;
            UserId = 0;
            User = new User();
            ManagerId = 0;
        }

        /// <summary>
        /// 带有完全参数的构造函数
        /// </summary>
        /// <param name="commentId">留言ID</param>
        /// <param name="content">留言内容</param>
        /// <param name="createTime">创建时间</param>
        /// <param name="blogId">博客ID</param>
        /// <param name="blog">评论所属博客</param>
        /// <param name="fatherID">父评论ID</param>
        /// <param name="userId">用户ID</param>
        /// <param name="user">用户</param>
        /// <param name="managerId">管理员ID</param>
        public Comment(uint commentId, string content, DateTime createTime, uint blogId, Blog blog,uint fatherID ,uint userId, User user, uint managerId)
        {
            CommentId = commentId;
            Content = content;
            CreateTime = createTime;
            BlogID = blogId;
            FatherBlog = blog;
            FatherID = fatherID;
            UserId = userId;
            User = user;
            ManagerId = managerId;
        }

    }
}
