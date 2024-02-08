using System.Security.Claims;

namespace WebApp;

public static class Extensions
{
    public static Guid GetRequiredUserId(this HttpContext httpContext)
        => httpContext.User.GetRequiredUserId();

    public static void HtmxRetarget(this HttpContext httpContext, string selector)
        => httpContext.Response.Headers.Append("HX-RETARGET", selector);

    public static Guid GetRequiredUserId(this ClaimsPrincipal user)
        => Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
