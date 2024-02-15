using Novle.Domain.Entities.Base;

namespace Novle.Domain.Entities;

public class Archive : AuditableEntity
{
    public int BookId { get; set; }
    public Book Book { get; set; } = null!;
    
    public int UserId { get; set; }
}