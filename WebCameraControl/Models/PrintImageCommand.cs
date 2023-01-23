namespace WebCameraControl.Models;

public record PrintImageCommand
{
    public string? Base64ImageSource { get; init; }
    public int Count { get; init; } = 1;
}
