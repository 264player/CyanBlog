using System.Text;
using CyanBlog.DbAccess.Context;
using CyanBlog.Filter;
using CyanBlog.Middleware;
using Humanizer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog.Extensions.Logging;

namespace CyanBlog
{
    /// <summary>
    /// ����������
    /// </summary>
    public class Program
    {
        /// <summary>
        /// �������ڷ���
        /// </summary>
        /// <param name="args">�����в���</param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add<VisitFilter>();
            });

            builder.Services.AddSingleton<VisitFilter>();

            // ����cyanBlog���ݿ�������
            builder.Services.AddDbContext<CyanBlogDbContext>(options =>
            {
                options.UseMySql(builder.Configuration.GetConnectionString("MySQLConnection"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySQLConnection")));
                options.EnableSensitiveDataLogging();
                options.EnableServiceProviderCaching();
            }, ServiceLifetime.Transient); // ����ָ��������������ڣ���Transient

            //����Nlog��־
            builder.Services.AddLogging(logbuilder =>
            {
                logbuilder.ClearProviders();
                logbuilder.SetMinimumLevel(LogLevel.Trace);
                logbuilder.AddNLog("D:\\VSProjects\\CyanBlog\\CyanBlog\\NLog.config");
            });

            //����JWT�����֤
            var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
             {
                 options.RequireHttpsMetadata = false;
                 options.SaveToken = true;
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidIssuer = builder.Configuration["Jwt:Issuser"],
                     ValidAudience = builder.Configuration["Jwt:Audience"],
                     IssuerSigningKey = new SymmetricSecurityKey(key),

                     ValidateIssuer = false,
                     ValidateAudience = false,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = false,

                     ClockSkew = TimeSpan.FromSeconds(0),
                 };

                 options.Events = new JwtBearerEvents
                 {
                     OnMessageReceived = context =>
                     {
                         var token = context.Request.Cookies["JwtToken"];
                         if (!string.IsNullOrEmpty(token))
                         {
                             context.Token = token;
                         }
                         return Task.CompletedTask;
                     },
                     OnAuthenticationFailed = context =>
                     {
                         context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                         context.Response.ContentType = "application/json";
                         return Task.CompletedTask;
                     }
                 };
             });

            builder.Services.AddAuthorization();

            builder.Services.AddTransient<AdminCheckMiddleware>();

            var app = builder.Build();

            app.UseMiddleware<AdminCheckMiddleware>();

            using(var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CyanBlogDbContext>();
                dbContext.Database.Migrate();
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.Use(async (context, next) =>
            {
                var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogInformation("Request received");
                foreach (var cookie in context.Request.Cookies)
                {
                    logger.LogInformation($"Cookie: {cookie.Key} = {cookie.Value}");
                }
                await next();
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Blog}/{action=Index}/{id?}");

            app.Run();
        }
    }
}