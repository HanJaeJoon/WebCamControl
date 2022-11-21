using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using WebCameraControl.Core;
using WebCameraControl.Models;

namespace WebCameraControl.Controllers;

[ApiController]
[Route("api/image")]
public class ImageController : Controller
{
    private readonly AppDbContext _appDbContext;
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _environment;

    public ImageController(AppDbContext appDbContext, IConfiguration configuration, IHostEnvironment environment)
    {
        _appDbContext = appDbContext;
        _configuration = configuration;
        _environment = environment;
    }

    [HttpPost("send")]
    public async Task<JsonResult> SendImages(SendImagesCommand command)
    {
        if (command?.ImageSourceList is null || command.Email is null)
        {
            throw new Exception("잘못된 접근입니다.");
        }

        // JJ: 이미지 생성 로직
        byte[] resultBytes = Convert.FromBase64String(command.ImageSourceList.FirstOrDefault() ?? string.Empty);

        // DB 저장
        string downloadKey = Guid.NewGuid().ToString();
        string link;

        try
        {
            _appDbContext.UserFiles.Add(new UserFile
            {
                Id = _appDbContext.UserFiles.ToList().Count + 1,
                Email = command.Email,
                DownloadKey = downloadKey,
                Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Content = resultBytes
            });

            await _appDbContext.SaveChangesAsync();
        }
        catch
        {
            throw new Exception("DB 저장 실패");
        }

        try
        {
            // 최종 결과
            using MemoryStream memoryStream = new(resultBytes);

            ContentType contentType = new(MediaTypeNames.Image.Jpeg);
            Attachment attachment = new(memoryStream, contentType);

            link = $"{Request.GetUri().GetLeftPart(UriPartial.Authority)}/download/{downloadKey}";

            MailMessage newMail = new();

            newMail.From = new MailAddress(_configuration["EmailSenderAddress"] ?? string.Empty,
                _configuration["EmailSenderName"]);
            newMail.To.Add(command.Email);

            newMail.IsBodyHtml = true;
            newMail.Subject = "[HaruHaru] pictures";
            newMail.Body = $@"<h1><a href=""{link}"">click to download</h1>";

            newMail.Attachments.Add(attachment);

            SmtpClient client = new("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(_configuration["GmailUser"], _configuration["GmailPassword"]),
                EnableSsl = true,
            };

            client.Send(newMail);
        }
        catch
        {
            throw new Exception("Email 전송 실패");
        }

        return Json(link);
    }
}
