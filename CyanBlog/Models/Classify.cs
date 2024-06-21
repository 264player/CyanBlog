using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyanBlog.Models
{
    /// <summary>
    /// 为博客分类
    /// </summary>
    public class Classify
    {
        /// <summary>
        /// 分类ID
        /// </summary>
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Display(Name = "类别Id")]
        public uint ClassId {  get; set; }

        /// <summary>
        /// 类别名
        /// </summary>
        [StringLength(100)]
        [Display(Name = "类别名")]
        public string Name {  get; set; }

        /// <summary>
        /// 默认的构造方法
        /// </summary>
        public Classify()
        {
            Name = string.Empty;
            ClassId = 0;
        }

        /// <summary>
        /// 带有完全参数的构造方法
        /// </summary>
        /// <param name="name">分类名</param>
        /// <param name="id">分类ID</param>
        public Classify(string name,uint id)
        {
            Name = name;
            ClassId = id;
        }
    }
}
