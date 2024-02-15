using Novle.Domain.Entities.Base;

namespace Novle.Domain.Entities;

public class Chapter : AuditableEntity
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public double Index { get; set; }
    public int WordCount { get; set; }
    
    public int BookId { get; set; }
    public Book Book { get; set; } = null!; 
}