using System.ComponentModel.DataAnnotations;

namespace NCDC.WebBootstrapper.Controllers.ActualDatabase.Create;

public class ActualDatabaseCreateRequest
{
    
    /// <summary>
    /// 所属逻辑数据库名称
    /// </summary>
    [Display(Name = "逻辑数据库"),Required(ErrorMessage = "{0}不能为空")]
    public string LogicDatabaseId { get; set; } = null!;
    /// <summary>
    /// 数据源名称
    /// </summary>
    [Display(Name = "数据源名称"),Required(ErrorMessage = "{0}不能为空")]
    public string DataSourceName { get; set; } = null!;
    /// <summary>
    /// 数据源链接
    /// </summary>
    [Display(Name = "数据源链接"),Required(ErrorMessage = "{0}不能为空")]
    public string ConnectionString { get; set; } = null!;

    /// <summary>
    /// 是否默认数据源
    /// </summary>
    public bool IsDefault { get; set; }
}