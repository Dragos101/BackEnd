using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data {
  public class AuthDbContext: IdentityDbContext {

    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        var readerRoleId = "316f7dac-bd63-41ee-85da-205279f60027";
        var writerRoleId = "367f3e34-36f5-4f74-ade4-78f8c055909e";
        
        // Create Reader and writer role
        var roles = new List<IdentityRole> {
          new IdentityRole() {
            Id = readerRoleId,
            Name = "Reader",
            NormalizedName = "Reader".ToUpper(),
            ConcurrencyStamp = readerRoleId
          },
          new IdentityRole() {
            Id = writerRoleId,
            Name = "Writer",
            NormalizedName = "Writer".ToUpper(),
            ConcurrencyStamp = writerRoleId
          }
        };
        // seed the roles
        builder.Entity<IdentityRole>().HasData(roles);

        // create an Admin user
        var adminUserId = "5147c1cc-c9dc-4bcc-8e4f-55600d30e5fd";
        var admin = new IdentityUser() {
          Id = adminUserId,
          UserName = "admin@training.com",
          Email = "admin@training.com",
          NormalizedEmail = "admin@training.com".ToUpper(),
          NormalizedUserName = "admin@training.com".ToUpper()
        };

        admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Ada123!");
        builder.Entity<IdentityUser>().HasData(admin);

        // Give role to admin
        var adminRoles = new List<IdentityUserRole<string>>() {
          new() {
            UserId = adminUserId,
            RoleId = readerRoleId,
          },
          new() {
            UserId = adminUserId,
            RoleId = writerRoleId,
          }
        };

        builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
    }

  }
}