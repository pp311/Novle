using AutoMapper;

namespace Novle.Application.ViewModels.Chapter.Responses;

public class GetChapterResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    
    public int BookId { get; set; }
    public string BookTitle { get; set; } = null!;
    
    public int WordCount { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
    
    private class Mapping : Profile 
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Chapter, GetChapterResponse>()
                .ForMember(d => d.BookTitle, opt => opt.MapFrom(s => s.Book.Title));
        }
    }
}