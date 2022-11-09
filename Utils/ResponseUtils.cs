using System.Net;
using FeedAppApi.Errors;
using Microsoft.AspNetCore.Mvc;

namespace FeedAppApi.Utils;

public static class ResponseUtils
{
    public static UnauthorizedObjectResult UnauthorizedResponse(string message)
    {
        return new UnauthorizedObjectResult(new ApiErrorResponse
        {
            Status = HttpStatusCode.Unauthorized,
            Message = message
        });
    }

    public static NotFoundObjectResult NotFoundResponse(string message)
    {
        return new NotFoundObjectResult(new ApiErrorResponse
        {
            Status = HttpStatusCode.NotFound,
            Message = message
        });
    }
}