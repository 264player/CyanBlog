using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CyanBlog.Controllers
{
    /// <summary>
    /// 管理文件的控制器
    /// </summary>
    public class FileController : Controller
    {
        /// <summary>
        /// 日志
        /// </summary>
        private ILogger<FileController> _logger;

        /// <summary>
        /// 带有日志的图片构造器方法
        /// </summary>
        /// <param name="logger">文件控制器日志</param>
        public FileController(ILogger<FileController> logger)
        { 
            _logger = logger;
        }

        /// <summary>
        /// 展示上传的图片以及路径
        /// </summary>
        /// <returns>展示图片页面</returns>
        [Authorize]
        public ActionResult Index()
        {
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

            if (!Directory.Exists(imagePath))
            {
                return NotFound();
            }

            List<string> files = Directory.GetFiles(imagePath).Where(file => new[] { ".jpg", ".jpeg", ".png", ".gif" }.Contains(Path.GetExtension(file).ToLower()))
                .Select(file => "localhost/images/" + Path.GetFileName(file)).ToList();
            return View(files);
        }

        /// <summary>
        /// 上传图片的界面
        /// </summary>
        /// <returns>上传图片页面</returns>
        [Authorize]
        [HttpGet]
        public ActionResult UploadImage()
        {

            return View(); 
        }

        /// <summary>
        /// 上传图片的接口
        /// 上传成功或失败的信息存放到ViewData["UploadInfo"]中
        /// </summary>
        /// <param name="image">表单中上传的图片文件</param>
        /// <returns>上传成功则继续返回上传界面，并返回成功信息。如果失败就返回失败的信息。</returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> UploadImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                ViewData["UploadInfo"] = "上传图片不能为空！";
                return View();
            }

            string[] permittedExtensions = new string[] { ".jpg","jpeg",".png",".gif" };
            string extension = Path.GetExtension(image.FileName).ToLowerInvariant();

            if(string.IsNullOrEmpty(extension) || !permittedExtensions.Contains(extension))
            {
                ViewData["UploadInfo"] = "上传图片格式有误！";
                return View();
            }

            string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            string fileName = Path.GetRandomFileName() + extension;
            string filePath = Path.Combine(savePath, fileName);

            using(var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            ViewData["UploadInfo"] = "上传成功！";
            return View();
        }

    }
}
