using FluentValidation;
using Novle.Domain.Constants;

namespace Novle.Application.ViewModels.Chapter.Requests;

public class CreateChapterRequest : IRequest
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!; 
    
    public double? Index { get; set; }
}

public class CreateChapterValidator : AbstractValidator<CreateChapterRequest>
{
    public CreateChapterValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(StringLength.Name);
        RuleFor(x => x.Content).NotEmpty();
        RuleFor(x => x.Index).GreaterThan(0).When(x => x.Index != null);
    }
}