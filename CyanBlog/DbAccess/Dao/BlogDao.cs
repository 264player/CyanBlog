using CyanBlog.DbAccess.Context;
using CyanBlog.DbAccess.Interface;
using CyanBlog.Models;

namespace CyanBlog.DbAccess.Dao
{
    /// <summary>
    /// 博客表数据库访问层
    /// </summary>
    public class BlogDao : IDao<Blog>
    {
        /// <summary>
        /// CyanBlog数据库上下文
        /// </summary>
        public CyanBlogDbContext Context { get; set; }

        /// <summary>
        /// 使用数据库上下文初始化博客表数据库访问层
        /// </summary>
        /// <param name="context"></param>
        public BlogDao(CyanBlogDbContext context)
        {
            Context = context;
        }

        ///<inheritdoc cref="IDao{T}.Add(T)"/>
        public bool Add(Blog entity)
        {
            Context.Blog.Add(entity);
            return Save();
        }

        ///<inheritdoc cref="IDao{T}.Count"/>
        public int Count()
        {
            int count = Context.Blog.Count();
            return count;
        }

        ///<inheritdoc cref="IDao{T}.Delet(T)"/>
        public bool Delet(Blog entity)
        {
            Context.Blog.Remove(entity);
            return Save();
        }

        ///<inheritdoc cref="IDao{T}.GetAll"/>
        public List<Blog> GetAll()
        {
            return Context.Blog.ToList<Blog>();
        }

        ///<inheritdoc cref="IDao{T}.RemoveAt(int)"/>
        public bool RemoveAt(int index)
        {
            Blog? blog = Select(index);
            if(blog != null)
            {
                Context.Blog.Remove(blog);
                return Save();
            }
            return false;
        }

        ///<inheritdoc cref="IDao{T}.Select(int)"/>
        public Blog? Select(int index)
        {
            Blog? blog = Context.Blog.First<Blog>();
            return blog;
        }

        ///<inheritdoc cref="IDao{T}.Update(T)"/>
        public bool Update(Blog entity)
        {
            Context.Blog.Update(entity);
            return Save();
        }

        /// <summary>
        /// 将更改保存到数据库
        /// </summary>
        /// <returns>保存是否成功</returns>
        public bool Save()
        {
            bool success = Context.SaveChanges() > 0;
            return success;
        }
    }
}
