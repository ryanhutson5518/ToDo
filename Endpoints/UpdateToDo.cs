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

public class UpdateToDo : IEndpoint
{
    public string Pattern => "/update-todo";

    public HttpMethod HttpMethod => HttpMethod.Post;

    public Delegate Handler => [Authorize] async (
        HttpContext httpContext,
        [FromForm] ToDoDto dto,
        [FromServices] DatabaseContext databaseContext,
        [FromServices] IValidator validator) =>
    {
        ToDo? entity = null;
        var parameters = new Dictionary<string, object?>();

        try
        {
            entity = dto.Id == default ? null : await databaseContext.ToDos
                .Include(t => t.ToDoItems)
                .FirstOrDefaultAsync(t => t.Id == dto.Id);

            entity ??= new();
            entity.Title = dto.Title;

            var validationResult = validator.Validate(dto);
            if (validationResult.IsValid == false)
            {
                parameters.Add(nameof(ToDoModalForm.ModalConfig), Modal.BuildModalConfig(entity.Id));
                parameters.Add(nameof(ToDoModalForm.Entity), entity);
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
                
                foreach (var (toDoItemDto, index) in dto.ToDoItems.Select((toDoItemDto, index) => (toDoItemDto, index)))
                {
                    // create
                    if (toDoItemDto.Id == default)
                    {
                        entity.ToDoItems.Add(new ToDoItem
                        {
                            Id = Guid.NewGuid(),
                            CreateDate = createModifyDate,
                            ModifyDate = createModifyDate,
                            Status = toDoItemDto.IsCompleted ? ToDoStatus.Completed : ToDoStatus.NotCompleted,
                            Description = toDoItemDto.Description,
                            SortOrder = index,
                        });
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
                    // delete
                    entity.ToDoItems.RemoveAll(e => dto.ToDoItems.Any(d => d.Id == e.Id) == false);
                }
            }

            await databaseContext.SaveChangesAsync();

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
            parameters.Add(nameof(ToDoModalForm.ModalConfig), Modal.BuildModalConfig(entity?.Id ?? default));
            parameters.Add(nameof(ToDoModalForm.Entity), entity ?? new());
            parameters.Add(nameof(ToDoModalForm.ServerErrors), serverErrors);

            httpContext.HtmxRetarget("closest form");
            return (RazorComponentResult)new RazorComponentResult<ToDoModalForm>(parameters)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    };
}
