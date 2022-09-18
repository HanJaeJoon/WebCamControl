using Microsoft.AspNetCore.Mvc;

namespace WebCameraControl.Controllers
{
    [ApiController]
    public class PictureController : Controller
    {
        [HttpGet("/send")]
        [HttpPost("/send")]
        public void SendPicture()
        {
        }
    }
}
