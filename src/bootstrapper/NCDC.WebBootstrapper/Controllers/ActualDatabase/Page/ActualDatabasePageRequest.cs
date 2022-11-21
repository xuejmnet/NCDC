using System.ComponentModel.DataAnnotations;

namespace NCDC.WebBootstrapper.Controllers.ActualDatabase.Page;

public class ActualDatabasePageRequest:PageRequest
{
    /// <summary>
    /// 所属逻辑数据库名称
    /// </summary>
    [Display(Name = "逻辑数据库"),Required(ErrorMessage = "请先选择{0}")]
    public string LogicDatabaseId { get; set; } = null!;
    
    public string? DataSourceName { get; set; }
}