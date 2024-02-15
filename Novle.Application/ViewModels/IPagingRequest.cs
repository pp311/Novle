using System.ComponentModel;
using Novle.Domain.Constants;

namespace Novle.Application.ViewModels;

public class IPagingRequest : IRequest
{
    [DefaultValue(AppConstant.DefaultPageNumber)]
    public int PageNumber { get; set; } = AppConstant.DefaultPageNumber;
    
    [DefaultValue(AppConstant.DefaultPageSize)]
    public int PageSize { get; set; } = AppConstant.DefaultPageSize;
    
    public string? Search { get; set; }
    
    [DefaultValue(false)]
    public bool IsDescending { get; set; } = false;
}