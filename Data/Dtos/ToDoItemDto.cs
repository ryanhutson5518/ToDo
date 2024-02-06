namespace WebApp.Data;

public class ToDoItemDto
{
    public Guid Id { get; set; }

    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// True if the user checked the checkbox
    /// </summary>
    public bool IsCompleted { get; set; }
}
