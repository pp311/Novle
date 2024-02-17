using FluentValidation;

namespace Novle.Application.ViewModels.Chapter.Requests;

public class UpdateChapterIndexRequest : IRequest
{
    public double Index { get; set; } 
}

public class UpdateChapterIndexValidator : AbstractValidator<UpdateChapterIndexRequest>
{
    public UpdateChapterIndexValidator()
    {
        RuleFor(x => x.Index).GreaterThan(0);
    }
}