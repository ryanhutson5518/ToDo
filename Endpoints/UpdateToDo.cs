//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Components.Forms;
//using Microsoft.AspNetCore.Http.HttpResults;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Collections.ObjectModel;
//using System.Net;
//using WebApp.Components.Home;
//using WebApp.Data;

//namespace WebApp.Endpoints;

//public class UpdateToDo : IEndpoint
//{
//    public string Pattern => "/validate-todo/{toDoId:guid}";

//    public HttpMethod HttpMethod => HttpMethod.Post;

//    public Delegate Handler => [Authorize] async (
//        HttpContext httpContext,
//        [FromRoute] Guid toDoId,
//        [FromForm] Form form,
//        [FromServices] DatabaseContext databaseContext) =>
//    {
//        // validation
//        if (string.IsNullOrWhiteSpace(form.Input.Title))
//        {
//            var editContext = new EditContext(form.Input);
//            var validationStore = new ValidationMessageStore(editContext);

//            validationStore.Add(() => form.Input, "Title can't be empty");

//            var parameters = new ReadOnlyDictionary<string, object?>(new Dictionary<string, object?>
//            {
//                { nameof(ToDoFormFields.Input), form.Input },
//                { nameof(EditContext), editContext },
//            });
//            return new RazorComponentResult<ToDoFormFields>(parameters)
//            {
//                StatusCode = (int)HttpStatusCode.BadRequest
//            };
//        }

//        var toDo = toDoId == default ? null : await databaseContext.ToDos
//            .AsNoTracking()
//            .FirstOrDefaultAsync(t => t.Id == toDoId);

//        toDo ??= new();

//        var createModifyDate = DateTimeOffset.UtcNow;
//        toDo.ModifyDate = createModifyDate;
//        toDo.Title = form.Input.Title;

//        if (toDo.Id == default)
//        {
//            toDo.Id = Guid.NewGuid();
//            toDo.CreateDate = createModifyDate;
//            toDo.Status = ToDoStatus.NotCompleted;
//            toDo.UserId = httpContext.GetRequiredUserId();

//            databaseContext.Add(toDo);
//            await databaseContext.SaveChangesAsync();
//        }
//        else
//        {
//            await databaseContext.ToDos
//                .Where(toDo => toDo.Id == toDo.Id)
//                .ExecuteUpdateAsync(x => x
//                    .SetProperty(toDo => toDo.Title, toDo.Title)
//                    .SetProperty(toDo => toDo.ModifyDate, toDo.ModifyDate));
//        }

//        return Results.Redirect("/");
//    };

//    private class Form
//    {
//        public ToDoModalEditForm.InputModel Input { get; set; } = new();
//    }
//}
