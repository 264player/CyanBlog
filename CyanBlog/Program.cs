using System.Text;
using CyanBlog.BackGroudService;
using CyanBlog.DbAccess.Context;
using CyanBlog.Filter;
using CyanBlog.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MQ;
using NLog.Extensions.Logging;
using RabbitMQ.Client;

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

            //��Ϣ���з���
            builder.Services.AddSingleton(sp =>
            {
                return new ConnectionManager(builder.Configuration.GetConnectionString("RqbbitMqHost"));
            });
            builder.Services.AddSingleton<MessagePublisher>();
            builder.Services.AddSingleton<MessageConsumer>();

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
                         return Task.CompletedTask;
                     }
                 };
             });

            #region rabbitmq

            var factory = new ConnectionFactory() { HostName = "localhost" }; // ���� RabbitMQ ��ַ
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            // ��������
            channel.QueueDeclare(queue: "viewcount_queue",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            // �� RabbitMQ �ŵ�ע�����
            builder.Services.AddSingleton(channel);

            builder.Services.AddHostedService<RabbitMqConsumerService>();

            #endregion

            builder.Services.AddAuthorization();

            builder.Services.AddTransient<AdminCheckMiddleware>();
            builder.Services.AddTransient<ErrorHandlingMiddleware>();

            var app = builder.Build();

            //�м������
           

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
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    // ���û���ͷ
                    ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=86400");
                    ctx.Context.Response.Headers.Append("Expires", DateTime.UtcNow.AddDays(1).ToString("R"));
                }
            });

            app.UseRouting();



            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMiddleware<AdminCheckMiddleware>();


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Blog}/{action=Index}/{id?}");

            app.Run();
        }
    }
}