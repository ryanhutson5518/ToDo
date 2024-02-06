using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebApp.Components.Home;
using WebApp.Components.Shared;
using WebApp.Data;
using WebApp.Services;

namespace WebApp.Endpoints;

public class AddToDoItemRow : IEndpoint
{
    public string Pattern => "/add-todoitem-row";

    public HttpMethod HttpMethod => HttpMethod.Post;

    public Delegate Handler => [Authorize] (
        HttpContext httpContext,
        [FromForm] ToDoDto dto,
        [FromServices] IValidator validator) =>
    {
        var parameters = new Dictionary<string, object?>();
        var entity = new ToDo
        {
            Id = dto.Id,
            Title = dto.Title,
            ToDoItems = dto.ToDoItems.ConvertAll(i => new ToDoItem
            {
                Id = i.Id,
                Description = i.Description,
                Status = i.IsCompleted ? ToDoStatus.Completed : ToDoStatus.NotCompleted,
            })
        };

        try
        {
            var validationResult = validator.Validate(dto);
            if (validationResult.IsValid == false)
            {
                // Add global error to let user know that they need to fix any errors before adding new row
                validationResult.Errors.TryAdd(ToDoItems.AddRowButtonName, ["Please fix errors above before adding a new row"]);

                parameters.Add(nameof(ToDoModalForm.ModalConfig), Modal.BuildModalConfig(entity.Id));
                parameters.Add(nameof(ToDoModalForm.Entity), entity);
                parameters.Add(nameof(ToDoModalForm.ServerErrors), validationResult.Errors);

                httpContext.HtmxRetarget("closest form");
                return (RazorComponentResult)new RazorComponentResult<ToDoModalForm>(parameters)
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }

            // Add new row
            entity.ToDoItems.Add(new());

            parameters.Add(nameof(ToDoItems.Entity), entity);

            httpContext.HtmxRetarget($"#{ToDoItems.WrapperId}");
            return new RazorComponentResult<ToDoItems>(parameters);
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
