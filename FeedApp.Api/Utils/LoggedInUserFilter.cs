using FeedApp.Api.Services;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FeedApp.Api.Utils;

public class LoggedInUserFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        var userService = filterContext.HttpContext.RequestServices.GetService<IUserService>();
        var currentUser = userService!.GetLoggedInUser(filterContext.HttpContext);
        filterContext.HttpContext.Items["user"] = currentUser;
    }
}