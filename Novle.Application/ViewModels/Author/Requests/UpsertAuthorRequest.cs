using FluentValidation;
using Novle.Domain.Constants;

namespace Novle.Application.ViewModels.Author.Requests;

public class UpsertAuthorRequest :IRequest
{
	public string Name { get; set; } = null!;
	public DateTime? BirthDay { get; set; }
	public string? Description { get; set; }
	public string? AvatarUrl { get; set; }
}

public class UpsertBookValidator : AbstractValidator<UpsertAuthorRequest>
{
    public UpsertBookValidator()
    {
	    RuleFor(_ => _.Name).NotEmpty().MaximumLength(StringLength.Name);
	    RuleFor(_ => _.Description).MaximumLength(StringLength.Description);
	    RuleFor(_ => _.AvatarUrl).MaximumLength(StringLength.Url);
	    RuleFor(_ => _.BirthDay).LessThan(DateTime.Now);
    }
}
