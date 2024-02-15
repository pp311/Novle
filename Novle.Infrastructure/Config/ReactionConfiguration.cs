using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Novle.Domain.Entities;
using Novle.Domain.Enums;

namespace Novle.Infrastructure.Config;

public class ReactionConfiguration : IEntityTypeConfiguration<Reaction>
{
    public void Configure(EntityTypeBuilder<Reaction> builder)
    {
        builder.Property(r => r.Type).IsRequired();

        builder.Property(r => r.Type).HasConversion(
            v => v.ToString(),
            v => (ReactionType)Enum.Parse(typeof(ReactionType), v));
        
        builder.HasOne(r => r.Comment)
            .WithMany(c => c.Reactions)
            .HasForeignKey(r => r.CommentId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(r => r.Review)
            .WithMany(c => c.Reactions)
            .HasForeignKey(r => r.ReviewId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
    }
}