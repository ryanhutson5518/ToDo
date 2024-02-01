namespace ToDo.Data;

public class ToDoItem : IKeyedModel, IDatedModel
{
    public Guid Id { get; set; }

    public DateTimeOffset CreateDate { get; set; }

    public DateTimeOffset ModifyDate { get; set; }

    public ToDoStatus Status { get; set; }

    public virtual ToDo ToDo { get; set; } = new();
}
