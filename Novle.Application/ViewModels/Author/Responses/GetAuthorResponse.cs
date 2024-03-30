namespace Novle.Application.ViewModels.Author.Responses;

public class GetAuthorResponse
{
	public int Id { get; set; }	
    public string Name { get; set; } = null!;
    public DateTime? BirthDay { get; set; }
    public string? Description { get; set; }
    public string? AvatarUrl { get; set; }
}