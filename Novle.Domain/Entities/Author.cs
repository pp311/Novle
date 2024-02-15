using Novle.Domain.Entities.Base;

namespace Novle.Domain.Entities;

public class Author : AuditableEntity, ISoftDelete
{
    public string Name { get; set; } = null!;
    public DateTime? BirthDay { get; set; }
    public string? Description {get;set;}
    public string? AvatarUrl { get; set; }
    
    public bool IsDeleted { get; set; }

    public ICollection<Book> Books { get; set; } = new HashSet<Book>();
}