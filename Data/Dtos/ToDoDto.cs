namespace WebApp.Data;

public class ToDoDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public List<ToDoItemDto> ToDoItems { get; set; } = [];
}
