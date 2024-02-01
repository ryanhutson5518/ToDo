using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ToDo.Data;

public class DatabaseContext(
    DbContextOptions<DatabaseContext> options)
    : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>(
        options)
{
    public DbSet<ToDo> ToDos { get; set; } = default!;

    public DbSet<ToDoItem> ToDoItems { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>(options =>
        {
            options.ToTable("Users");
        });

        builder.Entity<Role>(options =>
        {
            options.ToTable("Roles");
        });

        builder.Entity<UserClaim>(options =>
        {
            options.ToTable("UserClaims");
        });

        builder.Entity<UserToken>(options =>
        {
            options.ToTable("UserTokens");
        });

        builder.Entity<UserLogin>(options =>
        {
            options.ToTable("UserLogins");
        });

        builder.Entity<RoleClaim>(options =>
        {
            options.ToTable("RoleClaims");
        });

        builder.Entity<UserRole>(options =>
        {
            options.ToTable("UserRoles");
        });

        builder.Entity<ToDo>(options =>
        {
        });

        builder.Entity<ToDoItem>(options =>
        {
        });
    }

    private static void OnModelCreatingCustom(ModelBuilder builder)
    {
        foreach (var type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()))
        {
            if (type is IKeyedModel)
            {
                builder.Entity(type, options =>
                {
                    options.HasKey(nameof(IKeyedModel.Id))
                        .IsClustered(false);
                });
            }
            if (type is IDatedModel)
            {
                builder.Entity(type, options =>
                {
                    // Default clustered index
                    options.HasIndex(nameof(IDatedModel.CreateDate))
                        .IsClustered(true)
                        .IsDescending();

                    options.HasIndex(nameof(IDatedModel.ModifyDate));
                });
            }
        }
    }
}
