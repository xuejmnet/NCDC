using System.ComponentModel.DataAnnotations;

namespace NCDC.WebBootstrapper.Controllers.ActualDatabase.All;

public class ActualDatabaseAllRequest
{
    /// <summary>
    /// 所属逻辑数据库名称
    /// </summary>
    [Display(Name = "逻辑数据库"),Required(ErrorMessage = "{0}不能为空")]
    public string LogicDatabaseId { get; set; } = null!;
}