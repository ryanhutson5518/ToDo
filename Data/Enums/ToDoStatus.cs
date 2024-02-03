namespace WebApp.Data;

public enum ToDoStatus
{
    /// <summary>
    /// A "ToDo" that is NOT completed yet
    /// </summary>
    NotCompleted,

    /// <summary>
    /// A "ToDo" that is completed
    /// </summary>
    Completed,

    /// <summary>
    /// A "ToDo" that is thrown in the trash
    /// </summary>
    Trashed,
}
