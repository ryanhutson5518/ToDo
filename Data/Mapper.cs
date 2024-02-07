namespace WebApp.Data;

public static class Mapper
{
    public static ToDoDto MapToDo(ToDo entity) => new()
    {
        Id = entity.Id,
        Title = entity.Title,
        ToDoItems = entity.ToDoItems.ConvertAll(MapToDoItem),
    };

    public static ToDoItemDto MapToDoItem(ToDoItem entity) => new()
    {
        Id = entity.Id,
        Description = entity.Description,
        IsCompleted = entity.Status == ToDoStatus.Completed,
    };
}
