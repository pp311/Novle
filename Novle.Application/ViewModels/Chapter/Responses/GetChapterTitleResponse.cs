using AutoMapper;

namespace Novle.Application.ViewModels.Chapter.Responses;

public class GetChapterTitleResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public double Index { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Chapter, GetChapterTitleResponse>();
        }
    }
}