using System.Security.Claims;

namespace ToDo;

public static class HttpContextExtensions
{
    public static Guid GetRequiredUserId(this HttpContext httpContext)
        => Guid.Parse(httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
