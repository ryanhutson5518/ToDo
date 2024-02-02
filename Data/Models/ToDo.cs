﻿namespace ToDo.Data;

public class ToDo : IKeyedModel, ITimestampedModel, IToDoStatus
{
    public Guid Id { get; set; }

    public DateTimeOffset CreateDate { get; set; }

    public DateTimeOffset ModifyDate { get; set; }

    public ToDoStatus Status { get; set; }

    public string Title { get; set; } = string.Empty;

    public virtual List<ToDoItem> ToDoItems { get; set; } = [];
}
