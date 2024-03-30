using AutoMapper;
using Novle.Domain.Enums;

namespace Novle.Application.ViewModels.Book.Responses;

public class GetBookInfoResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? CoverUrl { get; set; }
    public BookStatus Status { get; set; }
    
    public int AuthorId { get; set; }
    public string? AuthorName { get; set; }
    
    public DateTime? CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }     
    
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Book, GetBookInfoResponse>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.Name));
        }
    }
}