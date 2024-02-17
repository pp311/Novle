using System.ComponentModel;
using Novle.Domain.Constants;

namespace Novle.Application.ViewModels;

public class PagingRequest : IRequest
{
    [DefaultValue(AppConstant.DefaultPageNumber)]
    public int PageNumber { get; set; } = AppConstant.DefaultPageNumber;
    
    [DefaultValue(AppConstant.DefaultPageSize)]
    public int PageSize { get; set; } = AppConstant.DefaultPageSize;
    
    [DefaultValue(false)]
    public bool IsDescending { get; set; } = false;
}