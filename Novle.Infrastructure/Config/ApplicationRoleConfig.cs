using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Novle.Infrastructure.Config;

public class ApplicationRoleConfiguration : IEntityTypeConfiguration<IdentityRole<int>>
{
    public void Configure(EntityTypeBuilder<IdentityRole<int>> builder)
    {
        builder.HasData(
            new IdentityRole<int>
            {
                Id = 1,
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new IdentityRole<int>
            {
                Id = 2,
                Name = "Editor",
                NormalizedName = "EDITOR"
            },
            new IdentityRole<int>
            {
                Id = 3,
                Name = "User",
                NormalizedName = "USER"
            }
        );
    }
}
