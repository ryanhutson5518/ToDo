namespace ToDo.Data;

public class ToDoItem : IKeyedModel, ITimestampedModel, IToDoStatusModel
{
    public Guid Id { get; set; }

    public DateTimeOffset CreateDate { get; set; }

    public DateTimeOffset ModifyDate { get; set; }

    public ToDoStatus Status { get; set; }

    public string Description { get; set; } = string.Empty;

    public Guid ToDoId { get; set; }

    public virtual ToDo ToDo { get; set; } = default!;
}
