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
        ConfigureModels(builder);

        builder.Entity<User>(options =>
        {
            options.ToTable("Users");

            options.HasMany(user => user.ToDos)
                .WithOne(toDo => toDo.User)
                .HasForeignKey(toDo => toDo.UserId)
                .OnDelete(DeleteBehavior.Cascade);
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
            options.HasMany(toDo => toDo.ToDoItems)
                .WithOne(toDoItem => toDoItem.ToDo)
                .HasForeignKey(toDoItem => toDoItem.ToDoId)
                .OnDelete(DeleteBehavior.Cascade);

            options.Property(toDo => toDo.Title)
                .IsRequired()
                .HasMaxLength(Constants.ToDoTitleMaxLength);
        });

        builder.Entity<ToDoItem>(options =>
        {
            options.Property(toDoItem => toDoItem.Description)
                .IsRequired()
                .HasMaxLength(Constants.ToDoItemDesciptionMaxLength);
        });
    }

    private static void ConfigureModels(ModelBuilder builder)
    {
        var types = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && t.IsAbstract == false);

        foreach (var type in types)
        {
            if (type.IsAssignableTo(typeof(IKeyedModel)))
            {
                builder.Entity(type, options =>
                {
                    options.HasKey(nameof(IKeyedModel.Id))
                        .IsClustered(false);
                });
            }
            if (type.IsAssignableTo(typeof(ITimestampedModel)))
            {
                builder.Entity(type, options =>
                {
                    // Default clustered index
                    options.HasIndex(nameof(ITimestampedModel.CreateDate))
                        .IsClustered(true)
                        .IsDescending();

                    options.HasIndex(nameof(ITimestampedModel.ModifyDate));
                });
            }
            if (type.IsAssignableTo(typeof(IToDoStatus)))
            {
                builder.Entity(type, options =>
                {
                    options.HasIndex(nameof(IToDoStatus.Status));
                });
            }
        }
    }
}
