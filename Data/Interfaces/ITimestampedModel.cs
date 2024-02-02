namespace ToDo.Data;

public interface ITimestampedModel
{
    DateTimeOffset CreateDate { get; set; }

    DateTimeOffset ModifyDate { get; set; }
}
