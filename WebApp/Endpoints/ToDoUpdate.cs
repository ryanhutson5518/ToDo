using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using WebApp.Components.Home;
using WebApp.Components.Shared;
using WebApp.Data;
using WebApp.Services;

namespace WebApp.Endpoints;

public class ToDoUpdate : IEndpoint
{
    public string Pattern => Constants.ToDoUpdatePath;

    public HttpMethod HttpMethod => HttpMethod.Post;

    public Delegate Handler => [Authorize] async (
        HttpContext httpContext,
        [FromForm] ToDoDto dto,
        [FromServices] DatabaseContext databaseContext,
        [FromServices] IValidator validator,
        CancellationToken cancellationToken = default) =>
    {
        ToDo? entity = null;
        var parameters = new Dictionary<string, object?>();

        try
        {
            entity = dto.Id == default ? null : await databaseContext.ToDos
                .Include(t => t.ToDoItems)
                .FirstOrDefaultAsync(t => t.Id == dto.Id, cancellationToken);

            entity ??= new();
            entity.Title = dto.Title;

            var validationResult = validator.Validate(dto);
            if (validationResult.IsValid == false)
            {
                parameters.Add(nameof(ToDoModalForm.ModalConfig), Modal.BuildModalConfig(dto.Id));
                parameters.Add(nameof(ToDoModalForm.Model), dto);
                parameters.Add(nameof(ToDoModalForm.ServerErrors), validationResult.Errors);

                httpContext.HtmxRetarget("closest form");
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
                entity.ToDoItems = dto.ToDoItems
                    .Select((toDoItemDto, index) => new ToDoItem
                    {
                        Id = Guid.NewGuid(),
                        CreateDate = createModifyDate,
                        ModifyDate = createModifyDate,
                        Status = toDoItemDto.IsCompleted ? ToDoStatus.Completed : ToDoStatus.NotCompleted,
                        Description = toDoItemDto.Description,
                        SortOrder = index,
                    }).ToList();

                databaseContext.Add(entity);
            }
            else
            {
                entity.ModifyDate = createModifyDate;

                // delete
                entity.ToDoItems.RemoveAll(e => e.Id != default && dto.ToDoItems.Any(d => d.Id == e.Id) == false);
                
                foreach (var (toDoItemDto, index) in dto.ToDoItems.Select((toDoItemDto, index) => (toDoItemDto, index)))
                {
                    // create
                    if (toDoItemDto.Id == default)
                    {
                        var newToDoItem = new ToDoItem
                        {
                            Id = Guid.NewGuid(),
                            CreateDate = createModifyDate,
                            ModifyDate = createModifyDate,
                            Status = toDoItemDto.IsCompleted ? ToDoStatus.Completed : ToDoStatus.NotCompleted,
                            Description = toDoItemDto.Description,
                            SortOrder = index,
                        };
                        entity.ToDoItems.Add(newToDoItem);
                        databaseContext.Add(newToDoItem);
                    }
                    // edit
                    else
                    {
                        var toDoItem = entity.ToDoItems.First(i => i.Id == toDoItemDto.Id);
                        toDoItem.ModifyDate = createModifyDate;
                        toDoItem.Status = toDoItemDto.IsCompleted ? ToDoStatus.Completed : ToDoStatus.NotCompleted;
                        toDoItem.Description = toDoItemDto.Description;
                        toDoItem.SortOrder = index;
                    }
                }
            }

            await databaseContext.SaveChangesAsync(cancellationToken);

            parameters.Add(nameof(ToDos.UserId), httpContext.GetRequiredUserId());

            httpContext.HtmxRetarget($"#{ToDos.WrapperId}");
            return (RazorComponentResult)new RazorComponentResult<ToDos>(parameters)
            {
                PreventStreamingRendering = true
            };
        }
        catch
        {
            // log exception here

            var serverErrors = new Dictionary<string, HashSet<string>>
            {
                { Constants.ServerErrorGlobal, [Constants.ServerErrorGlobalMessage] }
            };
            parameters.Add(nameof(ToDoModalForm.ModalConfig), Modal.BuildModalConfig(dto.Id));
            parameters.Add(nameof(ToDoModalForm.Model), dto);
            parameters.Add(nameof(ToDoModalForm.ServerErrors), serverErrors);

            httpContext.HtmxRetarget("closest form");
            return (RazorComponentResult)new RazorComponentResult<ToDoModalForm>(parameters)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    };
}
