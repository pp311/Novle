using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Novle.Domain.Entities;
using Novle.Domain.Enums;

namespace Novle.Infrastructure.Config;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.Property(c => c.Content).IsRequired();
        
        builder.Property(c => c.EntityType).HasConversion(
            v => v.ToString(),
            v => (CommentableEntityType)Enum.Parse(typeof(CommentableEntityType), v));
        
        builder.HasIndex(c => new { c.EntityId, c.EntityType })
            .IsUnique();
    }
}