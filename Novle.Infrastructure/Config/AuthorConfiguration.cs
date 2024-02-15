using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Novle.Domain.Constants;
using Novle.Domain.Entities;

namespace Novle.Infrastructure.Config;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.Property(a => a.Name).IsRequired().HasMaxLength(StringLength.Name);
        builder.Property(a => a.BirthDay).IsRequired(false);
        builder.Property(a => a.Description).HasMaxLength(StringLength.Description).IsRequired(false);
        builder.Property(a => a.AvatarUrl).HasMaxLength(StringLength.Url).IsRequired(false);
        
        builder.HasMany(a => a.Books)
            .WithOne(b => b.Author)
            .HasForeignKey(b => b.AuthorId);
    }
}