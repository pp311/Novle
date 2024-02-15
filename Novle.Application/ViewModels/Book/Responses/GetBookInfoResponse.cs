using AutoMapper;
using Novle.Domain.Enums;

namespace Novle.Application.ViewModels.Book.Responses;

public class GetBookInfoResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? PosterUrl { get; set; }
    public BookStatus Status { get; set; }
    
    public int AuthorId { get; set; }
    public string? AuthorName { get; set; }
    
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }     
    
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Book, GetBookInfoResponse>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.Name));
        }
    }
}