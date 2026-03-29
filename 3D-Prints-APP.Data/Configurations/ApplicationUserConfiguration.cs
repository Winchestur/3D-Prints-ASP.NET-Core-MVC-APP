using _3DPrintsAPP.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3DPrintsAPP.Data.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasData(new ApplicationUser
            {
                Id = "admin-user-id",
                UserName = "admin@site.com",
                NormalizedUserName = "ADMIN@SITE.COM",
                Email = "admin@site.com",
                NormalizedEmail = "ADMIN@SITE.COM",
                EmailConfirmed = true,
                SecurityStamp = "admin-security-stamp",
                ConcurrencyStamp = "admin-concurrency-stamp",
                PasswordHash = "AQAAAAIAAYagAAAAEMgoypDgbqDzlnw4UlwgU6TYJT73IMHdtY2JV0a668gEYJDfxImXXZQSuODkFJHRWA=="
            });
        }
    }
}