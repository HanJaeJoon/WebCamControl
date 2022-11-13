namespace WebCameraControl.Core;

public static class HttpRequestExtension
{
    public static Uri GetUri(this HttpRequest request)
    {
        UriBuilder uriBuilder = new()
        {
            Scheme = request.Scheme,
            Host = request.Host.Host,
            Port = request.Host.Port.GetValueOrDefault(-1),
            Path = request.Path.ToString(),
            Query = request.QueryString.ToString(),
        };

        return uriBuilder.Uri;
    }
}
