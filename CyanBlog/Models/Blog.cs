using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyanBlog.Models
{
    /// <summary>
    /// 博客类
    /// </summary>
    public class Blog
    {
        /// <summary>
        /// 博客ID
        /// </summary>
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name ="")]
        public uint BlogID { get; set; }

        /// <summary>
        /// 博客标题
        /// </summary>
        [Required(ErrorMessage = "这里忘记写了")]
        [MaxLength(50,ErrorMessage = "写多了")]
        [Display(Name ="标题")]
        public string Title {  get; set; }

        /// <summary>
        /// 博客描述
        /// </summary>
        [Required(ErrorMessage = "这里忘记写了")]
        [StringLength(300,ErrorMessage = "写多了")]
        [Display(Name ="博客描述")]
        public string Description {  get; set; }

        /// <summary>
        /// 博客正文
        /// </summary>
        [Display(Name = "博客正文")]
        [Required(ErrorMessage = "这里忘记写了")]
        public string Content {  get; set; }

        /// <summary>
        /// 博客创建时间
        /// </summary>
        [Required(ErrorMessage = "这里忘记写了")]
        [Display(Name ="创建时间")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 博客更改时间
        /// </summary>
        [Required(ErrorMessage = "这里忘记写了")]
        [Display(Name ="最后一次更改时间")]
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 是否可以评论
        /// </summary>
        [Required(ErrorMessage = "这里忘记写了")]
        [Display(Name ="开启评论")]
        public bool IsComment {  get; set; }

        /// <summary>
        /// 是否发布
        /// </summary>
        [Required(ErrorMessage = "这里忘记写了")]
        [Display(Name = "是否发布")]
        public bool IsPublish {  get; set; }

        /// <summary>
        /// 版权开启
        /// </summary>
        [Required(ErrorMessage = "这里忘记写了")]
        [Display(Name = "版权开放")]
        public bool CopyRight {  get; set; }

        /// <summary>
        /// 是否赞赏
        /// </summary>
        [Required(ErrorMessage = "这里忘记写了")]
        [Display(Name = "开启赞赏")]
        public bool Donate {  get; set; }

        /// <summary>
        /// 查看次数
        /// </summary>
        [Required(ErrorMessage = "这里忘记写了")]
        public uint ViewCount {  get; set; }

        /// <summary>
        /// 博客分类ID
        /// </summary>
        [Required]
        [Display(Name ="分类编号")]
        public uint ClassId {  get; set; }

        /// <summary>
        /// 博客类别
        /// </summary>
        [ForeignKey("ClassId")]
        public Classify? Classify { get; set; }

        /// <summary>
        /// 这个评论的集合
        /// </summary>
        public List<Comment>? Comments { get; set; }
        /// <summary>
        /// 默认的构造函数
        /// </summary>
        public Blog()
        {
            BlogID = 0;
            Title = string.Empty;
            Description = string.Empty;
            Content = string.Empty;
            CreateTime = DateTime.Now;
            UpdateTime = DateTime.Now;
            IsComment = false;
            IsPublish = false;
            CopyRight = false;
            Donate = false;
            ViewCount = 0;
            ClassId = 0;
            Classify = new Classify();
        }

        /// <summary>
        /// 带有完全参数的构造函数
        /// </summary>
        /// <param name="blogID">博客ID</param>
        /// <param name="title">博客标题</param>
        /// <param name="description">博客详情</param>
        /// <param name="content">博客内容</param>
        /// <param name="createTime">博客创建时间</param>
        /// <param name="updateTime">博客更新时间</param>
        /// <param name="isComment">是否可以评论</param>
        /// <param name="isPublish">是否发布</param>
        /// <param name="copyRight">版权开启</param>
        /// <param name="donate">捐赠开启</param>
        /// <param name="viewCount">浏览次数</param>
        /// <param name="classId">分类Id</param>
        public Blog(uint blogID, string title, string description, string content, DateTime createTime, DateTime updateTime, bool isComment, bool isPublish, bool copyRight, bool donate, uint viewCount, uint classId)
        {
            BlogID = blogID;
            Title = title;
            Description = description;
            Content = content;
            CreateTime = createTime;
            UpdateTime = updateTime;
            IsComment = isComment;
            IsPublish = isPublish;
            CopyRight = copyRight;
            Donate = donate;
            ViewCount = viewCount;
            ClassId = classId;
        }
    }
}
