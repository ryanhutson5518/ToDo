using System.ComponentModel.DataAnnotations;

namespace ToDo.Data;

public interface IKeyedModel
{
    [Required]
    Guid Id { get; set; }
}
