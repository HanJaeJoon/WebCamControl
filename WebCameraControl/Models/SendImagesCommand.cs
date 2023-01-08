namespace WebCameraControl.Models;

public record SendImagesCommand
{
    public string? Email { get; init; }
    public string? ImageSource { get; init; }
    public string? Width { get; init; }
    public string? Height { get; init; }
}
