using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using WebApp.Components.Home;
using WebApp.Data;

namespace WebApp.Endpoints;

public class ToDoItemUpdateStatus : IEndpoint
{
    public string Pattern => $"{Constants.ToDoItemUpdateStatusPath}";

    public HttpMethod HttpMethod => HttpMethod.Post;

    public Delegate Handler => [Authorize] async (
        HttpContext httpContext,
        [FromForm] Dto dto,
        [FromServices] DatabaseContext databaseContext,
        CancellationToken cancellationToken = default) =>
    {
        var parameters = new Dictionary<string, object?>();

        try
        {
            var newStatus = dto.IsCompleted ? ToDoStatus.NotCompleted : ToDoStatus.Completed;

            await databaseContext.ToDoItems
                .Where(i => i.Id == dto.ToDoItemId)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(i => i.Status, newStatus)
                    .SetProperty(i => i.ModifyDate, DateTimeOffset.UtcNow),
                    cancellationToken);

            parameters.Add(nameof(ToDoItemListItem.ToDoItemId), dto.ToDoItemId);
            parameters.Add(nameof(ToDoItemListItem.Description), dto.Description);
            parameters.Add(nameof(ToDoItemListItem.IsCompleted), newStatus == ToDoStatus.Completed);

            httpContext.HtmxRetarget("closest form");
            return new RazorComponentResult<ToDoItemListItem>(parameters);
        }
        catch
        {
            // log exception here

            var serverErrors = new Dictionary<string, HashSet<string>>
            {
                { Constants.ServerErrorGlobal, [Constants.ServerErrorGlobalMessage] }
            };
            parameters.Add(nameof(ToDoItemListItem.ToDoItemId), dto.ToDoItemId);
            parameters.Add(nameof(ToDoItemListItem.Description), dto.Description);
            // Since an unexpected error occured, we don't want to update the checkbox
            parameters.Add(nameof(ToDoItemListItem.IsCompleted), dto.IsCompleted);
            parameters.Add(nameof(ToDoItemListItem.ServerErrors), serverErrors);

            httpContext.HtmxRetarget("closest form");
            return new RazorComponentResult<ToDoItemListItem>(parameters)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    };

    public class Dto
    {
        public Guid ToDoItemId { get; set; }

        public string Description { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }
    }
}
