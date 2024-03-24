using Novle.Domain.Entities.Base;
using Novle.Domain.Enums;

namespace Novle.Domain.Entities;

public class Book : AuditableEntity, ISoftDelete
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? CoverUrl { get; set; }
    public int ViewCount { get; set; }
    public BookStatus Status { get; set; }
    
    public int AuthorId { get; set; }
    public Author Author { get; set; } = null!;
    public bool IsDeleted { get; set; }
    public ICollection<Genre> Genres { get; set; } = new HashSet<Genre>();
    public ICollection<Chapter> Chapters { get; set; } = new HashSet<Chapter>();
    public ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
    public ICollection<Archive> Archives { get; set; } = new HashSet<Archive>();

    private Book() { }
    
    private Book(string title, string? description, string? coverUrl, int authorId, List<int> genreIds)
    {
        Title = title;
        Description = description;
        CoverUrl = coverUrl;
        AuthorId = authorId;
        Status = BookStatus.OnGoing;
        Genres = genreIds.Select(id => new Genre { Id = id }).ToList();
    }
    
    public static Book Create(
        string title, 
        string? description, 
        string? coverUrl, 
        int authorId, 
        List<int> genreIds)
    {
        return new Book(title, description, coverUrl, authorId, genreIds);
    }
    
    public void Update(string title, string? description, string? coverUrl, int authorId, List<int> genreIds)
    {
        Title = title;
        Description = description;
        CoverUrl = coverUrl;
        AuthorId = authorId;
        Genres = genreIds.Select(id => new Genre { Id = id }).ToList();
    }
    
    public void Delete()
    {
        // Todo: add domain event
    }
    
    public void IncreaseViewCount() => ViewCount++;
}