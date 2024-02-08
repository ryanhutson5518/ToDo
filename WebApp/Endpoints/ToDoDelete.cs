using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using WebApp.Components.Home;
using WebApp.Components.Shared;
using WebApp.Data;

namespace WebApp.Endpoints;

public class ToDoDelete : IEndpoint
{
    public string Pattern => $"{Constants.ToDoDeletePath}/{{toDoId:guid}}";

    public HttpMethod HttpMethod => HttpMethod.Delete;

    public Delegate Handler => [Authorize] async (
        HttpContext httpContext,
        [FromRoute] Guid toDoId,
        [FromServices] DatabaseContext databaseContext,
        CancellationToken cancellationToken = default) =>
    {
        var parameters = new Dictionary<string, object?>();

        try
        {
            await databaseContext.ToDos
                .Where(t => t.Id == toDoId)
                .ExecuteDeleteAsync(cancellationToken);

            parameters.Add(nameof(ToDos.UserId), httpContext.GetRequiredUserId());
            httpContext.HtmxRetarget($"#{ToDos.WrapperId}");
            return new RazorComponentResult<ToDos>(parameters)
            {
                PreventStreamingRendering = true,
            };
        }
        catch
        {
            // log exception here

            var forParameterValue = toDoId.ToString();

            var serverErrors = new Dictionary<string, HashSet<string>>
            {
                { forParameterValue, [Constants.ServerErrorGlobalMessage] }
            };
            parameters.Add(nameof(ServerValidationMessage.For), forParameterValue);
            parameters.Add(nameof(ServerValidationMessage.ServerErrors), serverErrors);

            httpContext.HtmxRetarget($"#{ServerValidationMessage.WrapperId(forParameterValue)}");
            return (RazorComponentResult)new RazorComponentResult<ServerValidationMessage>(parameters)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    };
}
