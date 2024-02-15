using Novle.Domain.Entities.Base;
using Novle.Domain.Enums;

namespace Novle.Domain.Entities;

public class Reaction : AuditableEntity
{
    public int EntityId { get; set; }
    public ReactionType Type { get; set; }
    public int? CommentId { get; set; }
    public Comment? Comment { get; set; }
    public int? ReviewId { get; set; }
    public Review? Review { get; set; }
}