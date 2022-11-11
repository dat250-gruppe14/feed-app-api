using System.Net;

namespace FeedApp.Api.Errors;

public class ApiErrorResponse
{
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public HttpStatusCode Status { get; set; }
    public string Error => Status.ToString();
    public string Message { get; set; }
}
