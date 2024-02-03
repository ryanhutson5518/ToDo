namespace WebApp.Data;

public class ToDo : IKeyedModel, ITimestampedModel, IToDoStatusModel
{
    public Guid Id { get; set; }

    public DateTimeOffset CreateDate { get; set; }

    public DateTimeOffset ModifyDate { get; set; }

    public ToDoStatus Status { get; set; }

    public string Title { get; set; } = string.Empty;

    public virtual List<ToDoItem> ToDoItems { get; set; } = [];

    public Guid UserId { get; set; }

    public virtual User User { get; set; } = default!;
}
