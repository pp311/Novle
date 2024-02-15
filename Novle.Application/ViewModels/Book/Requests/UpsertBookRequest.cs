using FluentValidation;
using Novle.Domain.Constants;

namespace Novle.Application.ViewModels.Book.Requests;

public class UpsertBookRequest : IRequest
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public int AuthorId { get; set; }
    public List<int> GenreIds { get; set; } = new();
    public string? CoverUrl { get; set; }
}

public class CreateBookValidator : AbstractValidator<UpsertBookRequest>
{
    public CreateBookValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(StringLength.Name);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(StringLength.Description);
        RuleFor(x => x.AuthorId).NotEmpty();
        RuleFor(x => x.GenreIds).NotEmpty();
        RuleFor(x => x.CoverUrl).MaximumLength(StringLength.Url);
    }
}