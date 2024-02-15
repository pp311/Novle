using Novle.Domain.Entities.Base;

namespace Novle.Domain.Entities;

public abstract class BaseComment : AuditableEntity
{
    public string Content { get; set; } = null!;
    public int UserId { get; set; }
}