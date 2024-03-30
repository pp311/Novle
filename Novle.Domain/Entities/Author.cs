using Novle.Domain.Entities.Base;

namespace Novle.Domain.Entities;

public class Author : AuditableEntity, ISoftDelete
{
    public string Name { get; set; } = null!;
    public DateTime? BirthDay { get; set; }
    public string? Description { get; set; }
    public string? AvatarUrl { get; set; }

    public bool IsDeleted { get; set; }

    public ICollection<Book> Books { get; set; } = new HashSet<Book>();
    
    public Author(string name, string? description, string? avatarUrl, DateTime? birthDay)
    {
        Name = name;
        Description = description;
        AvatarUrl = avatarUrl;
        BirthDay = birthDay;
    }
    
    public void Update(string name, string? description, string? avatarUrl, DateTime? birthDay)
    {
        Name = name;
        Description = description;
        AvatarUrl = avatarUrl;
        BirthDay = birthDay;
    }

    public void Delete()
    {
    }
}
