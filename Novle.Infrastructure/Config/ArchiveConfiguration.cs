using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Novle.Domain.Entities;

namespace Novle.Infrastructure.Config;

public class ArchiveConfiguration : IEntityTypeConfiguration<Archive>
{
    public void Configure(EntityTypeBuilder<Archive> builder)
    {
        builder.HasIndex(a => new { a.BookId, a.UserId }).IsUnique();
        
        builder.HasOne(a => a.Book)
            .WithMany(b => b.Archives)
            .HasForeignKey(a => a.BookId);
    }
}