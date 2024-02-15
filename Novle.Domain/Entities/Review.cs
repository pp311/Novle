namespace Novle.Domain.Entities;

public class Review : BaseComment
{
    public int Rating { get; set; }
    public int BookId { get; set; }
    public Book Book { get; set; } = null!;
    public ICollection<Reaction> Reactions { get; set; } = new HashSet<Reaction>();
}