using Novle.Domain.Enums;

namespace Novle.Domain.Entities;

public class Comment : BaseComment
{
    public int EntityId { get; set; }
    public CommentableEntityType EntityType { get; set; }
    public ICollection<Reaction> Reactions { get; set; } = new HashSet<Reaction>();
}