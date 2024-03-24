using Novle.Domain.Entities.Base;
using Novle.Domain.Repositories;

namespace Novle.Domain.Entities;

public class Chapter : AuditableEntity
{
    public string Title { get; set; }
    public string Content { get; set; }
    public double Index { get; set; }
    public int WordCount { get; set; }
    
    public int BookId { get; set; }
    public Book Book { get; set; } = null!;


    private Chapter(string title, string content, double index, int bookId)
    {
        Title = title;
        Content = content;
        BookId = bookId;
        WordCount = content.Split(" ").Length;
        Index = index;
    }
    
    public static async Task<Chapter> CreateAsync(
        string title, 
        string content, 
        double? index, 
        int bookId, 
        IChapterRepository chapterRepository)
    {
        var maxIndex = index ?? await chapterRepository.GetMaxIndexAsync(bookId) + 1;
        return new Chapter(title, content, maxIndex, bookId);
    }
    
    public void Update(string title, string content)
    {
        Title = title;
        if (Content != content)
        {
            Content = content;
            WordCount = content.Split(" ").Length;
        }
    }

    public void UpdateIndex(double index) => Index = index;
}