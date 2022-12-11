namespace WebCameraControl.Models;

public record SendImagesCommand
{
    public string? Email { get; init; }
    public IReadOnlyList<string>? ImageSourceList { get; init; }
}
