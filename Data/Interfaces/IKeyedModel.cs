using System.ComponentModel.DataAnnotations;

namespace WebApp.Data;

public interface IKeyedModel
{
    Guid Id { get; set; }
}
