namespace ToDo.Data;

public interface IDatedModel
{
    DateTimeOffset CreateDate { get; set; }

    DateTimeOffset ModifyDate { get; set; }
}
