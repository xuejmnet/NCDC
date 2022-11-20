using System.ComponentModel.DataAnnotations;

namespace NCDC.WebBootstrapper.Controllers.LogicTable.Update;

public class LogicTableUpdateRequest: IValidatableObject
{
    [Display(Name = "id"),Required(ErrorMessage = "{0}不能为空")]
    public string Id { get; set; } = null!;

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