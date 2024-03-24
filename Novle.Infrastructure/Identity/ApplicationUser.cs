using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Novle.Domain.Entities;

namespace Novle.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<int>
{
    public string? Introduction { get; set; }
    public string? AvatarUrl { get; set; }
    public string? Settings { get; set; }
    public DateTime? Birthday { get; set; }
    public string FullName { get; set; } = string.Empty;

    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ApplicationUser, User>().ReverseMap();
    }
}
