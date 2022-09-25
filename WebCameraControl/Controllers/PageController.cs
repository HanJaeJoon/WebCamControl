using Microsoft.AspNetCore.Mvc;
using WebCameraControl.Core;
using WebCameraControl.Models;

namespace WebCameraControl.Controllers
{
    public class PageController : Controller
    {
        private readonly IConfiguration _configuration;

        public PageController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("/")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/pay")]
        public IActionResult Pay()
        {
            return View();
        }

        [HttpPost("/check-password")]
        [ValidateAntiForgeryToken]
        public IActionResult CheckPassword([FromForm] string password)
        {
            string appPassword = _configuration["Password"];

            if (password != appPassword)
            {
                return Unauthorized("Unauthorized!");
            }

            HttpContext.Session.SetString("IsPasswordChecked", "1");

            return Redirect("/take-picture");
        }

        [CheckLogin]
        [HttpGet("/take-picture")]
        public IActionResult TakePicture()
        {
            return View(new IndexModel
            {
                CameraManufacturer = _configuration["CameraManufacturer"],
            });
        }
    }
}
