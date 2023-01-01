namespace WebCameraControl.Models;

public record PrintImageCommand
{
    public string? Base64ImageSource { get; init; }
}
