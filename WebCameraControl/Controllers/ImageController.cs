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

    [HttpPost("print")]
    public async Task<JsonResult> PrintImage([FromBody] PrintImageCommand command)
    {
        string? printerEmail = _configuration["PrinterEmail"];

        if (string.IsNullOrEmpty(command.Base64ImageSource) || string.IsNullOrEmpty(printerEmail))
        {
            throw new Exception("잘못된 접근입니다.");
        }

        byte[] resultBytes = Convert.FromBase64String(command.Base64ImageSource);

        // DB 저장
        string downloadKey = Guid.NewGuid().ToString();

        try
        {
            _appDbContext.UserFiles.Add(new UserFile
            {
                Id = _appDbContext.UserFiles.ToList().Count + 1,
                Email = printerEmail,
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
            MemoryStream image = new(resultBytes);

            MailMessage newMail = new();

            newMail.From = new MailAddress(_configuration["EmailSenderAddress"] ?? string.Empty,
                _configuration["EmailSenderName"]);
            newMail.To.Add(printerEmail);

            // 이미지 첨부를 위한 처리
            const string body = $@"<img src=""cid:HaruHaruPicture"" style=""width: 450px; height: 600px;"" alt=""image"" />";
            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);

            LinkedResource imageSource = new(image, MediaTypeNames.Image.Jpeg);
            imageSource.ContentId = "HaruHaruPicture";
            imageSource.ContentType = new ContentType("image/jpg");
            alternateView.LinkedResources.Add(imageSource);
            newMail.AlternateViews.Add(alternateView);

            ContentType mimeType = new("text/html");
            AlternateView alternate = AlternateView.CreateAlternateViewFromString(body, mimeType);
            newMail.AlternateViews.Add(alternate);

            newMail.Subject = "[HaruHaru] pictures";
            newMail.Body = body;

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

        return Json(downloadKey);
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
            MemoryStream image = new(resultBytes);

            link = $"{Request.GetUri().GetLeftPart(UriPartial.Authority)}/download/{downloadKey}";

            MailMessage newMail = new();

            newMail.From = new MailAddress(_configuration["EmailSenderAddress"] ?? string.Empty,
                _configuration["EmailSenderName"]);
            newMail.To.Add(command.Email);

            // 이미지 첨부를 위한 처리
            const string body = $@"<img src=""cid:HaruHaruPicture"" style=""width: 540px; height: 360px;"" alt=""image"" />";
            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);

            LinkedResource imageSource = new(image, MediaTypeNames.Image.Jpeg);
            imageSource.ContentId = "HaruHaruPicture";
            imageSource.ContentType = new ContentType("image/jpg");
            alternateView.LinkedResources.Add(imageSource);
            newMail.AlternateViews.Add(alternateView);

            ContentType mimeType = new("text/html");
            AlternateView alternate = AlternateView.CreateAlternateViewFromString(body, mimeType);
            newMail.AlternateViews.Add(alternate);

            newMail.Subject = "[HaruHaru] pictures";
            newMail.Body = body;

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
