using System.Globalization;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebCameraControl.Core;
using WebCameraControl.Models;

namespace WebCameraControl.Controllers;

public class FileController : Controller
{
    private readonly AppDbContext _appDbContext;

    public FileController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    [AllowAnonymous]
    [HttpGet("/download/{downloadKey}")]
    public IActionResult Download([FromRoute] string downloadKey)
    {
        UserFile? imageFile = _appDbContext.UserFiles.FirstOrDefault(x => x.DownloadKey == downloadKey);

        if (imageFile?.Content is null || !DateTime.TryParseExact(imageFile.Date, "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime writeDate))
        {
            return View("_Error", new ErrorModel
            {
                Message = "파일을 찾을 수 없습니다.",
            });
        }

        if (DateTime.Now > writeDate.AddDays(3))
        {
            _appDbContext.UserFiles.Remove(imageFile);
            _appDbContext.SaveChanges();

            return View("_Error", new ErrorModel
            {
                Message = "다운로드 가능한 기간이 지났습니다.",
            });
        }
        else
        {
            return File(imageFile.Content, "image/jpeg", $"{imageFile.Email}.jpg");
        }
    }
}
