using CyanBlog.DbAccess.Context;
using CyanBlog.Models;
using MQ;

namespace CyanBlog.MessageQueue
{
    /// <summary>
    /// 负责博客浏览量的增加
    /// </summary>
    public class BlogViewCountIncreament : IMessageHandler
    {

        /// <summary>
        /// 需要递增的博客id
        /// </summary>
        public uint _blogID;
        public CyanBlogDbContext? _dbContext;

        /// <summary>
        /// 使用博客id初始化此类
        /// </summary>
        /// <param name="blogID">需要递增浏览量的博客id</param>
        /// <param name="cyanBlogDbContext">cyanblog数据库上下文</param>
        public BlogViewCountIncreament(uint blogID,CyanBlogDbContext cyanBlogDbContext)
        {
            _blogID = blogID;
            _dbContext = cyanBlogDbContext;
        }

        /// <summary>
        /// <see cref="BlogViewCountIncreament"/>
        /// </summary>
        /// <param name="blogID"></param>
        public BlogViewCountIncreament(uint blogID)
        {
            _blogID = blogID;
        }

        public BlogViewCountIncreament() { }

        /// <summary>
        /// 浏览量+1
        /// </summary>
        /// <returns></returns>
        public async Task ExecuteAsync()
        {
            Blog? blog = await _dbContext.Blog.FindAsync(_blogID);
            if (blog != null)
            {
                blog.ViewCount++;
                _dbContext.SaveChanges();
            }
        }
    }
}
