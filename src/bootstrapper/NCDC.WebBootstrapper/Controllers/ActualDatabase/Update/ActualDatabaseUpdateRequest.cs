using System.ComponentModel.DataAnnotations;

namespace NCDC.WebBootstrapper.Controllers.ActualDatabase.Update;

public class ActualDatabaseUpdateRequest
{
    
    [Display(Name = "id"),Required(ErrorMessage = "{0}不能为空")]
    public string Id { get; set; } = null!;
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

}