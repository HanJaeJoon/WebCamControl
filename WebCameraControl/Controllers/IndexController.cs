using Microsoft.AspNetCore.Mvc;

namespace WebCameraControl.Controllers
{
    public class IndexController : Controller
    {
        [HttpGet("/")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
