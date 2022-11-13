namespace WebCameraControl.Models;

public class UserFile
{
    public int Id { get; set; }
    public string? Email { get; set; }
    public string? DownloadKey { get; init; }
    public string? Date { get; set; }
    public byte[]? Content { get; init; }
}
