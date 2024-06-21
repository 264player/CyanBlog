using CyanBlog.DbAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace CyanBlog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<CyanBlogDbContext>(options =>
            {
                options.UseMySql(builder.Configuration.GetConnectionString("MySQLConnection"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySQLConnection")));
                options.EnableSensitiveDataLogging();
                options.EnableServiceProviderCaching();
            }, ServiceLifetime.Transient); // 尝试指定服务的生命周期，如Transient


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
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}