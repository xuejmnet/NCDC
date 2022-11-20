
namespace NCDC.WebBootstrapper.Controllers.LogicTable.ActualTablePage;

public class ActualTablePageResponse
{
    
    public string Id { get; set; } = null!;
    public DateTime CreateTime { get; set; }
    public DateTime UpdateTime { get; set; }
    /// <summary>
    /// 表名
    /// </summary>
    public string TableName { get; set; } = null!;
    /// <summary>
    /// 数据库
    /// </summary>
    public string LogicDatabaseName { get; set; } = null!;
    /// <summary>
    /// 逻辑表名
    /// </summary>
    public string LogicTableName { get; set; } = null!;
    /// <summary>
    /// 数据源
    /// </summary>
    public string DataSource { get; set; } = null!;
}