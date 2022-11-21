using System.ComponentModel.DataAnnotations;

namespace NCDC.WebBootstrapper.Controllers.LogicTable.ActualTablePage;

public class ActualTablePageRequest:PageRequest
{
    
    /// <summary>
    /// 所属逻辑数据库名称
    /// </summary>
    [Display(Name = "逻辑数据库"),Required(ErrorMessage = "请先选择{0}")]
    public string LogicDatabaseId { get; set; } = null!;
    
    [Display(Name = "逻辑表"),Required(ErrorMessage = "请先选择{0}")]
    public string LogicTableId { get; set; } = null!;
}