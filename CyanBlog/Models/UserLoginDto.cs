namespace CyanBlog.Models
{
    /// <summary>
    /// 用户登录数据传输对象
    /// </summary>
    public class UserLoginDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 带有完全参数的构造函数
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        public UserLoginDto(string username, string password)
        {
            UserName = username;
            Password = password;
        }

        /// <summary>
        /// 默认的构造函数
        /// </summary>
        public UserLoginDto()
        {
            UserName = string.Empty;
            Password = string.Empty;
        }
    }
}
