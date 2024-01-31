using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ToDo.Data;

public class DatabaseContext(
    DbContextOptions<DatabaseContext> options)
    : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>(
        options)
{
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
    }
}
