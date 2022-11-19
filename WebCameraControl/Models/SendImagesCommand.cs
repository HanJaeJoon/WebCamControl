namespace WebCameraControl.Models;

public class SendImagesCommand
{
    public string? Email { get; set; }

    // ReSharper disable once CollectionNeverUpdated.Global
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public IList<string>? ImageSourceList { get; set; }
}
