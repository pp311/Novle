using Novle.Domain.Entities.Base;

namespace Novle.Domain.Entities;

public class Genre : Entity
{
    public string Name { get; set; } = null!;
    public ICollection<Book> Books { get; set; } = new HashSet<Book>();
}