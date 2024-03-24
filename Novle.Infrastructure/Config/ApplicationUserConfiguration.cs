using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Novle.Domain.Constants;
using Novle.Infrastructure.Identity;

namespace Novle.Infrastructure.Config;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasIndex(u => u.UserName).IsUnique();
        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.Settings)
            .HasMaxLength(StringLength.ConfigurationJson);

        builder.Property(u => u.FullName)
            .IsRequired()
            .HasMaxLength(StringLength.Name);

        builder.Property(u => u.Introduction)
            .HasMaxLength(StringLength.Description);

        builder.Property(u => u.AvatarUrl)
            .HasMaxLength(StringLength.Url);

        builder.HasData(
            new ApplicationUser
            {
                Id = 1,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "phucdk311@gmail.com",
                NormalizedEmail = "PHUCDK311@GMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null, "Admin@123"),
                SecurityStamp = Guid.NewGuid().ToString(),
                FullName = "Phuc DK",
            });
    }
}

public class ApplicationUserRoleConfig : IEntityTypeConfiguration<IdentityUserRole<int>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<int>> builder)
    {
        builder.HasData(
            new IdentityUserRole<int>
            {
                RoleId = 1,
                UserId = 1
            });
    }
}
