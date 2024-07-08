using CyanBlog.DbAccess.Context;
using Microsoft.EntityFrameworkCore;
using NLog.Extensions.Logging;

namespace CyanBlog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // 配置cyanBlog数据库上下文
            builder.Services.AddDbContext<CyanBlogDbContext>(options =>
            {
                options.UseMySql(builder.Configuration.GetConnectionString("MySQLConnection"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySQLConnection")));
                options.EnableSensitiveDataLogging();
                options.EnableServiceProviderCaching();
            }, ServiceLifetime.Transient); // 尝试指定服务的生命周期，如Transient

            //配置Nlog日志
            builder.Services.AddLogging(logbuilder =>
            {
                logbuilder.ClearProviders();
                logbuilder.SetMinimumLevel(LogLevel.Trace);
                logbuilder.AddNLog("D:\\VSProjects\\CyanBlog\\CyanBlog\\NLog.config");
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

           

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Blog}/{action=Index}/{id?}");

            app.Run();
        }
    }
}