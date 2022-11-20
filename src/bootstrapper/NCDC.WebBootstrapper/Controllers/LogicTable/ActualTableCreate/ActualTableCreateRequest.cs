using System.ComponentModel.DataAnnotations;

namespace NCDC.WebBootstrapper.Controllers.LogicTable.ActualTableCreate;

public class ActualTableCreateRequest
{
    
    /// <summary>
    /// 表名
    /// </summary>
    [Display(Name = "实际表名"), Required(ErrorMessage = "{0}不能为空")]
    public string TableName { get; set; } = null!;
    /// <summary>
    /// 数据库
    /// </summary>
    [Display(Name = "逻辑数据库"), Required(ErrorMessage = "{0}不能为空")]
    public string LogicDatabaseName { get; set; } = null!;
    /// <summary>
    /// 逻辑表名
    /// </summary>
    [Display(Name = "逻辑表名"), Required(ErrorMessage = "{0}不能为空")]
    public string LogicTableName { get; set; } = null!;
    /// <summary>
    /// 数据源
    /// </summary>
    [Display(Name = "数据源"), Required(ErrorMessage = "{0}不能为空")]
    public string DataSource { get; set; } = null!;
}