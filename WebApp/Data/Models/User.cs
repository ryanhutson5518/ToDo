using Microsoft.AspNetCore.Identity;

namespace WebApp.Data;

public class User : IdentityUser<Guid>
{
    public virtual List<ToDo> ToDos { get; set; } = [];
}
