using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using FluentValidation;

namespace Novle.Application.ViewModels.Book.Requests;

public class GetBooksRequest : IPagingRequest
{
    public int? AuthorId { get; set; }
    public int? GenreId { get; set; }
    [DefaultValue(BookSortByOption.Id)]
    public BookSortByOption SortBy { get; set; } = BookSortByOption.Id;
}

public class GetBooksValidator : AbstractValidator<GetBooksRequest>
{
    public GetBooksValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
        RuleFor(x => x.SortBy).IsInEnum().Must(BeAValidEnumValue).WithMessage("Invalid sort by option.");
    }

    private bool BeAValidEnumValue(BookSortByOption sortByOption)
    {
        return Enum.IsDefined(typeof(BookSortByOption), sortByOption.ToString());
    }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum BookSortByOption
{
    Id,
    Title,
    LatestUploaded,
    LatestUpdated,
    ViewCount,
    RatingScore,
    RatingCount,
    ArchivedCount,
}