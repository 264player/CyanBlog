using CyanBlog.DbAccess.Context;
using CyanBlog.Models;
using Microsoft.EntityFrameworkCore;

namespace CyanBlog.Middleware
{
    /// <summary>
    /// 检查管理员是否存在并且唯一的中间件
    /// </summary>
    public class AdminCheckMiddleware:IMiddleware
    {
        async Task IMiddleware.InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var dbContext = context.RequestServices.GetService(typeof(CyanBlogDbContext)) as CyanBlogDbContext;
            if(dbContext != null)
            {
                bool onlyAdmin = OnlyAdmin(dbContext), initialUrl = context.Request.Path == "/Admin/InitializeRoot";
                if (onlyAdmin)
                {
                    if (initialUrl)
                    {
                        context.Response.Redirect("/");
                        return;
                    }
                }
                else
                {
                    if (!initialUrl)
                    {
                        context.Response.Redirect("/Admin/InitializeRoot");
                        return;
                    }
                }
            }
            await next(context);
        }

        private bool OnlyAdmin(CyanBlogDbContext dbContext)
        {
            List<User> adminList = dbContext.User.Where(u => u.Type == Models.UserType.ROOT).ToList();
            bool isOnly = adminList.Count == 1;
            return isOnly;
        }

    }
}
