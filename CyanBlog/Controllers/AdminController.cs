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

        [Authorize]
        public IActionResult AdminPage()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserLoginDto model)
        {
            var authenticatedUser = AuthenticateUser(model);
            if (authenticatedUser != null)
            {
                var token = GenerateJwtToken(authenticatedUser);

                // 保存JWT到Cookie
                Response.Cookies.Append("JwtToken", token, new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.Now.AddHours(1)
                });

                return RedirectToAction("AdminPage", "Admin");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

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
