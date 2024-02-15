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
            .IsRequired()
            .HasMaxLength(StringLength.ConfigurationJson);
        
        builder.Property(u => u.FullName)
            .IsRequired()
            .HasMaxLength(StringLength.Name);

        builder.Property(u => u.Introduction)
            .HasMaxLength(StringLength.Description);

        builder.Property(u => u.AvatarUrl)
            .HasMaxLength(StringLength.Url);
    }
}