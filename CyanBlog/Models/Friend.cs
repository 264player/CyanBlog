using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyanBlog.Models
{
    /// <summary>
    /// 友链
    /// </summary>
    public class Friend
    {
        /// <summary>
        /// 友链ID
        /// </summary>
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Display(Name ="")]
        public uint FriendId {  get; set; }

        /// <summary>
        /// 友联标题
        /// </summary>
        [Required]
        [StringLength(100)]
        [Display(Name ="友链标题")]
        public string Title {  get; set; }

        /// <summary>
        /// 友联路径
        /// </summary>
        [Required]
        [Url]
        [Display(Name ="友联链接")]
        public string Url {  get; set; }

        /// <summary>
        /// 友联图片路径
        /// </summary>
        [Required]
        [Url]
        public string PictureUrl {  get; set; }

        /// <summary>
        /// 友联创建时间
        /// </summary>
        [Required]
        [Display(Name = "创建时间")]
        public DateTime CreateTime {  get; set; }

        /// <summary>
        /// 默认的构造函数
        /// </summary>
        public Friend()
        {
            FriendId = 0;
            Title = string.Empty;
            Url = string.Empty;
            PictureUrl = string.Empty;
            CreateTime = DateTime.Now;
        }

        /// <summary>
        /// 带有完全参数的构造函数
        /// </summary>
        /// <param name="friendId">友联ID</param>
        /// <param name="title">友链标题</param>
        /// <param name="url">友链路径</param>
        /// <param name="pictureUrl">友联图片路径</param>
        /// <param name="createTime">友联创建时间</param>
        public Friend(uint friendId, string title, string url, string pictureUrl, DateTime createTime)
        {
            FriendId = friendId;
            Title = title;
            Url = url;
            PictureUrl = pictureUrl;
            CreateTime = createTime;
        }
    }
}
