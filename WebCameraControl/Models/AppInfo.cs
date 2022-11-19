using Microsoft.EntityFrameworkCore;

namespace WebCameraControl.Models;

[Keyless]
public class AppInfo
{
    public int Version { get; set; }
    public string? Date { get; set; }
}
