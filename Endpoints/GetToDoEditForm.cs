using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using ToDo.Components.Home;
using ToDo.Data;

namespace ToDo.Endpoints;

public class GetToDoEditForm : IEndpoint
{
    public string Pattern => "/ToDoEditForm";

    public HttpMethod HttpMethod => HttpMethod.Get;

    public Delegate Handler => [Authorize] async (
        [FromQuery] Guid? toDoId,
        DatabaseContext databaseContext) =>
    {
        var toDo = toDoId.GetValueOrDefault() == default ? null : await databaseContext.ToDos
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == toDoId);

        toDo ??= new();

        var parameters = new ReadOnlyDictionary<string, object?>(new Dictionary<string, object?>
        {
            { nameof(ToDoForm.Entity), toDo },
        });

        return new RazorComponentResult<ToDoForm>(parameters);
    };
}
