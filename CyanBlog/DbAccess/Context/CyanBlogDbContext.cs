using CyanBlog.Models;
using Microsoft.EntityFrameworkCore;

namespace CyanBlog.DbAccess.Context
{
    /// <summary>
    /// CyanBlog数据库上下文,里面会保存每一张表对应的实体类DbSet
    /// </summary>
    public class CyanBlogDbContext:DbContext
    {
        ///<inheritdoc cref="DbContext.DbContext(DbContextOptions)"/>
        public CyanBlogDbContext(DbContextOptions<CyanBlogDbContext> options) : base(options) { }

        /// <summary>
        /// 博客表
        /// </summary>
        public DbSet<Blog> Blog { get; set; }

        /// <summary>
        /// 博客分类表
        /// </summary>
        public DbSet<Classify> Classify { get; set; }

        /// <summary>
        /// 评论表
        /// </summary>
        public DbSet<Comment> Comment { get; set; }

        /// <summary>
        /// 友链表
        /// </summary>
        public DbSet<Friend> Friend { get; set; }

        /// <summary>
        /// 留言表
        /// </summary>
        public DbSet<Message> Message { get; set; }

        /// <summary>
        /// 用户表
        /// </summary>
        public DbSet<User> User { get; set; }

    }
}
