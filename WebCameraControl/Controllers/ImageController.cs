using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using WebCameraControl.Models;

namespace WebCameraControl.Controllers;

[ApiController]
[Route("api/image")]
public class ImageController : Controller
{
    private readonly IConfiguration _configuration;

    public ImageController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("send")]
    public void SendImages(SendImagesCommand command)
    {
        if (command?.ImageSourceList is null)
        {
            throw new Exception("잘못된 접근입니다.");
        }

        MailMessage newMail = new();

        newMail.From = new MailAddress(_configuration["EmailSenderAddress"], _configuration["EmailSenderName"]);

        newMail.To.Add("jaejoon.han@crevisse.com");

        newMail.IsBodyHtml = true;
        newMail.Subject = "My First Email";
        newMail.Body = "<h1> This is my first Templated Email in C# </h1>";

        // 이미지 생성 로직
        // 최종 결과
        byte[] resultBytes = Convert.FromBase64String(command.ImageSourceList.FirstOrDefault() ?? string.Empty);

        using MemoryStream memoryStream = new(resultBytes);

        ContentType contentType = new(MediaTypeNames.Image.Jpeg);
        Attachment attachment = new(memoryStream, contentType);

        newMail.Attachments.Add(attachment);

        SmtpClient client = new("smtp.gmail.com", 587)
        {
            Credentials = new NetworkCredential(_configuration["GmailUser"], _configuration["GmailPassword"]),
            EnableSsl = true,
        };

        client.Send(newMail);
    }
}
