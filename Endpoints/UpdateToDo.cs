using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Frozen;
using System.Net;
using WebApp.Components.Home;
using WebApp.Components.Shared;
using WebApp.Data;

namespace WebApp.Endpoints;

public class UpdateToDo : IEndpoint
{
    public string Pattern => "/update-todo";

    public HttpMethod HttpMethod => HttpMethod.Post;

    public Delegate Handler => [Authorize] async (
        HttpContext httpContext,
        [FromForm] Dto dto,
        [FromServices] DatabaseContext databaseContext) =>
    {
        var entity = dto.Id == default ? null : await databaseContext.ToDos
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == dto.Id);

        entity ??= new();
        entity.Title = dto.Title;

        var serverErrors = new Dictionary<string, HashSet<string>>();
        var parameters = new Dictionary<string, object?>();

        // validation
        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            serverErrors.Add(nameof(ToDo.Title), new HashSet<string> { "Title is required" });

            parameters.Add(nameof(ToDoModalForm.ModalConfig), Modal.BuildModalConfig(entity.Id));
            parameters.Add(nameof(ToDoModalForm.Entity), entity);
            parameters.Add(nameof(ToDoModalForm.ServerErrors), serverErrors.ToFrozenDictionary(x => x.Key, x => x.Value.ToFrozenSet()));

            httpContext.Response.Headers.Append("HX-RETARGET", "closest form");
            return (RazorComponentResult)new RazorComponentResult<ToDoModalForm>(parameters)
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };
        }

        var createModifyDate = DateTimeOffset.UtcNow;
        entity.ModifyDate = createModifyDate;

        if (entity.Id == default)
        {
            entity.Id = Guid.NewGuid();
            entity.CreateDate = createModifyDate;
            entity.Status = ToDoStatus.NotCompleted;
            entity.UserId = httpContext.GetRequiredUserId();

            databaseContext.Add(entity);
            await databaseContext.SaveChangesAsync();
        }
        else
        {
            await databaseContext.ToDos
                .Where(t => t.Id == entity.Id)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(t => t.Title, entity.Title)
                    .SetProperty(t => t.ModifyDate, entity.ModifyDate));
        }

        parameters.Add(nameof(ToDos.UserId), httpContext.GetRequiredUserId());

        httpContext.Response.Headers.Append("HX-RETARGET", $"#{ToDos.WrapperId}");
        return (RazorComponentResult)new RazorComponentResult<ToDos>(parameters)
        {
            PreventStreamingRendering = true
        };
    };

    private class Dto
    {
        /// <inheritdoc cref="ToDo.Id"/>
        public Guid Id { get; set; }

        /// <inheritdoc cref="ToDo.Title"/>
        public string Title { get; set; } = string.Empty;
    }
}
