using Novle.Domain.Entities.Base;

namespace Novle.Infrastructure.Identity;

public class RefreshToken : AuditableEntity
{
    public string Token { get; set; } = null!;
    public DateTime Expires { get; set; }
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public int UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;
}