namespace WebApp.Data;

public class ToDoDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public DateTimeOffset CreateDate { get; set; }

    public List<ToDoItemDto> ToDoItems { get; set; } = [];
}
