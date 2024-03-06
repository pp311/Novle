using AutoMapper;

namespace Novle.Application.ViewModels.Genre.Requests;

public class GetGenreResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    
    public class Mapping : Profile 
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Genre, GetGenreResponse>();
        }
    }
}