using Microsoft.AspNetCore.Identity;

namespace ToDo.Data;

public class User : IdentityUser<Guid>
{
    public virtual List<ToDo> ToDos { get; set; } = [];
}
