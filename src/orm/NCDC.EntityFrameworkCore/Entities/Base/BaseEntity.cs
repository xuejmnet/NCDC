namespace NCDC.EntityFrameworkCore.Entities.Base;

public abstract class BaseEntity:IEntity,ICreateTime,IUpdateTime,IVersion,ILogicDelete
{
    public string Id { get; set; } = null!;
    public DateTime CreateTime { get; set; }
    public DateTime UpdateTime { get; set; }
    public string Version { get; set; } = null!;
    public bool IsDelete { get; set; }
}