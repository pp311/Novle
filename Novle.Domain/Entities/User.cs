using Novle.Domain.Entities.Base;

namespace Novle.Domain.Entities;

public class User : Entity
{
    public string UserName { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    
    public string? Introduction { get; set; }
    public string? AvatarUrl { get; set; }
    public string? Settings { get; set; }
    public DateTime? Birthday { get; set; }
}