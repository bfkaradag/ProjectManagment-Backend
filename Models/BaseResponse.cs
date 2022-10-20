using System.Net;
using System.Text.Json.Serialization;

namespace DSPro.Models;

public class BaseResponse<T>
{
    public BaseResponse(
        HttpStatusCode statusCode = HttpStatusCode.OK,
        FriendlyMessage friendlyMessage = null,
        T payload = default)
    {
        StatusCode = statusCode;
        Payload = payload;
        FriendlyMessage = friendlyMessage;
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public HttpStatusCode StatusCode { get; set; }

    public FriendlyMessage FriendlyMessage { get; set; }
    public T Payload { get; set; }
    public int ServerTime { get; set; } = (int) (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
}

 public class FriendlyMessage
{
    public string Title { get; set; }
    public string Message { get; set; }

}

