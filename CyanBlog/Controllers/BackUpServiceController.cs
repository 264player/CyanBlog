using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace CyanBlog.Controllers
{
    /// <summary>
    /// 数据库备份服务
    /// </summary>
    public class BackUpServiceController : Controller
    {
        private IConfiguration _configuration;
        public BackUpServiceController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //[Authorize]
        public IActionResult Index()
        {
            string backupFilePath = BackUp();
            byte[] fileBytes = System.IO.File.ReadAllBytes(backupFilePath);
            return File(fileBytes, "application/octet-stream", Path.GetFileName(backupFilePath));
        }

        private string BackUp()
        {
            string connectionString = _configuration.GetConnectionString("MySQLConnection");
            string backupFile = @"G:\studyfile\else\database_backup.sql"; // 保存备份文件路径

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        cmd.Connection = conn;
                        conn.Open();
                        mb.ExportToFile(backupFile);
                        conn.Close();
                        return backupFile;
                    }
                }
            }
        }
    }
}
