using FeedAppApi.Services;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FeedAppApi.Utils;

public class LoggedInDeviceFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        var deviceService = filterContext.HttpContext.RequestServices.GetService<IDeviceService>();
        var currentDevice = deviceService!.GetLoggedInDevice(filterContext.HttpContext);
		if (currentDevice is null || !(currentDevice.isActive ?? false)){
		  return;
		}
		var authorizationHeaders = filterContext.HttpContext.Request.Headers.Authorization.ToList()[0][7..];
		var isAuthorized = deviceService!.IsCorrectDeviceCredentials(currentDevice!, authorizationHeaders);
        if (isAuthorized)
        {
            filterContext.HttpContext.Items["device"] = currentDevice;
        }
    }
}
