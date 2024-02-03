using System.Security.Claims;

namespace WebApp;

public static class HttpContextExtensions
{
    public static Guid GetRequiredUserId(this HttpContext httpContext)
        => Guid.Parse(httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
