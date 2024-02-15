using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Novle.Domain.Constants;
using Novle.Domain.Entities;
using Novle.Domain.Enums;

namespace Novle.Infrastructure.Config;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.Property(b => b.Title).IsRequired().HasMaxLength(StringLength.Name);
        builder.Property(b => b.Description).IsRequired(false);
        builder.Property(b => b.CoverUrl).HasMaxLength(StringLength.Url).IsRequired(false);
        
        builder.Property(b => b.Status).HasConversion(
            v => v.ToString(),
            v => (BookStatus)Enum.Parse(typeof(BookStatus), v));
        
        builder.HasMany(b => b.Genres)
            .WithMany(g => g.Books)
            .UsingEntity<BookGenre>(
                b => b.HasOne(x => x.Genre).WithMany().HasForeignKey(x => x.GenreId),
                b => b.HasOne(x => x.Book).WithMany().HasForeignKey(x => x.BookId));
    }
}