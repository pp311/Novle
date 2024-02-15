using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Novle.Domain.Constants;
using Novle.Domain.Entities;

namespace Novle.Infrastructure.Config;

public class ChapterConfiguration : IEntityTypeConfiguration<Chapter>
{
    public void Configure(EntityTypeBuilder<Chapter> builder)
    {
        builder.Property(c => c.Title).IsRequired().HasMaxLength(StringLength.Name);
        builder.Property(c => c.Content).IsRequired();
        builder.Property(c => c.WordCount).IsRequired();
        
        builder.HasOne(c => c.Book)
            .WithMany(b => b.Chapters)
            .HasForeignKey(c => c.BookId)
            .OnDelete(DeleteBehavior.Cascade); 
    }
}