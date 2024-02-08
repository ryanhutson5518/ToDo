using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebApp.Components.Home;
using WebApp.Components.Shared;
using WebApp.Data;
using WebApp.Services;

namespace WebApp.Endpoints;

public class ToDoItemAddRow : IEndpoint
{
    public string Pattern => Constants.ToDoItemAddRowPath;

    public HttpMethod HttpMethod => HttpMethod.Post;

    public Delegate Handler => [Authorize] (
        HttpContext httpContext,
        [FromForm] ToDoDto dto,
        [FromServices] IValidator validator) =>
    {
        var parameters = new Dictionary<string, object?>();

        try
        {
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

            // Add new row
            dto.ToDoItems.Add(new());

            parameters.Add(nameof(ToDoItems.Model), dto);

            httpContext.HtmxRetarget($"#{ToDoItems.WrapperId(dto.Id)}");
            return new RazorComponentResult<ToDoItems>(parameters);
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
