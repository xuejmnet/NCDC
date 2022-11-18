using System.ComponentModel.DataAnnotations;

namespace NCDC.WebBootstrapper.Controllers;

public class PageRequest
{
    
    [Display(Name = "当前页")]
    [Range(1, int.MaxValue, ErrorMessage = "{0}必须为非零的正整数")]
    public int Page { get; set; }

    [Display(Name = "每页行数")]
    [Range(1, 500, ErrorMessage = "{0}的范围为{1}和{2}之间")]
    public int PageSize { get; set; }
}