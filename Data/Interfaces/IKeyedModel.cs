using System.ComponentModel.DataAnnotations;

namespace ToDo.Data;

public interface IKeyedModel
{
    Guid Id { get; set; }
}
