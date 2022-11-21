using System.ComponentModel.DataAnnotations;

namespace NCDC.WebBootstrapper.Controllers.LogicTable.Create;

public class LogicTableCreateRequest : IValidatableObject
{
    /// <summary>
    /// 逻辑表名称
    /// </summary>
    [Display(Name = "逻辑表名"), Required(ErrorMessage = "{0}不能为空")]
    public string TableName { get; set; } = null!;

    [Display(Name = "所属逻辑库表名"), Required(ErrorMessage = "{0}不能为空")]
    public string LogicDatabaseId { get; set; } = null!;

    /// <summary>
    /// 分库规则
    /// </summary>
    public string? DataSourceRule { get; set; }

    public string? DataSourceRuleParam { get; set; }

    /// <summary>
    /// 分表规则
    /// </summary>
    public string? TableRule { get; set; }

    public string? TableRuleParam { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (DataSourceRule == null && TableRule == null)
            yield return new ValidationResult("分库和分表规则不能同时为空");
    }
}