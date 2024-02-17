using FluentValidation;
using Novle.Domain.Constants;

namespace Novle.Application.ViewModels.Chapter.Requests;

public class UpdateChapterRequest : IRequest
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
}

public class UpdateChapterValidator : AbstractValidator<UpdateChapterRequest>
{
    public UpdateChapterValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(StringLength.Name);
        RuleFor(x => x.Content).NotEmpty();
    }
}