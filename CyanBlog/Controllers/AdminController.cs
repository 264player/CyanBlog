using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CyanBlog.DbAccess.Context;
using CyanBlog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CyanBlog.Controllers
{
    /// <summary>
    /// 管理员控制器
    /// </summary>
    public class AdminController : Controller
    {
        /// <summary>
        /// 日志
        /// </summary>
        private ILogger<BlogController> _logger;

        /// <summary>
        /// CyanBlog数据库上下文
        /// </summary>
        private CyanBlogDbContext _dbContext;

        /// <summary>
        /// 配置信息
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 带有完全参数的管理员控制器
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="dbContext"></param>
        /// <param name="configuration"></param>
        public AdminController(ILogger<BlogController> logger, CyanBlogDbContext dbContext, IConfiguration configuration)
        {
            _logger = logger;
            _dbContext = dbContext;
            _configuration = configuration;
        }

        /// <summary>
        /// 管理员界面
        /// </summary>
        /// <returns></returns>

        public IActionResult InitializeRoot()
        {
            return View();
        }

        /// <summary>
        /// 处理初始化管理员界面接口
        /// </summary>
        /// <param name="userLogin">管理员初始化的信息</param>
        /// <returns>重定向到博客首页</returns>
        [HttpPost]
        public IActionResult InitializeRoot(UserLoginDto userLogin)
        {
            User u = new User();
            u.UserName = userLogin.UserName;
            u.Password = userLogin.Password;
            u.Type = UserType.ROOT;
            _dbContext.User.Add(u);
            _dbContext.SaveChanges();
            return RedirectToAction("Index", "Blog");
        }

        /// <summary>
        /// 管理员登录界面
        /// </summary>
        /// <returns></returns>
        [Route("Admin/")]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 管理员登录之后的验证接口
        /// </summary>
        /// <param name="model">管理员登录信息</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult LoginNext(UserLoginDto model)
        {
            var authenticatedUser = AuthenticateUser(model);
            if (authenticatedUser != null)
            {
                var token = GenerateJwtToken(authenticatedUser);

                // 保存JWT到Cookie
                Response.Cookies.Append("JwtToken", token, new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.Now.AddHours(12)
                });

                return RedirectToAction("ViewList", "Blog");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        /// <summary>
        /// 用于验证是否存在对应的管理员用户
        /// </summary>
        /// <param name="loginDto">管理员登录信息</param>
        /// <returns>如果存在对应的管理员用户，则返回管理员用户对象，否则返回空</returns>
        private User? AuthenticateUser(UserLoginDto loginDto)
        {
            User? u = _dbContext.User.Single(u=>u.UserName==loginDto.UserName&&u.Password==loginDto.Password);
            // 假设这是用户身份验证的逻辑
            if (u!=null&&u.Type == UserType.ROOT)
            {
                return u;
            }
            return null;
        }

        /// <summary>
        /// 使用管理员用户信息生成jwt令牌
        /// </summary>
        /// <param name="user">管理员用户</param>
        /// <returns>返回jwt令牌</returns>
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.NickName),
                new Claim(ClaimTypes.Role, user.Type.ToString()),
                new Claim(ClaimTypes.Sid, user.UserId.ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                
                Issuer = _configuration["Jwt:Issuser"],
                Audience = _configuration["Jwt:Audience"]
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
