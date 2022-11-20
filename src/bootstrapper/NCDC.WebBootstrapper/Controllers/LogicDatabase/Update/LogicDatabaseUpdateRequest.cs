using System.ComponentModel.DataAnnotations;
using NCDC.Enums;

namespace NCDC.WebBootstrapper.Controllers.LogicDatabase.Update;

public class LogicDatabaseUpdateRequest
{
    [Display(Name = "id"),Required(ErrorMessage = "{0}不能为空")]
    public string Id { get; set; } = null!;

    /// <summary>
    /// 写操作数据库后自动使用写库链接防止读库链接未同步无法查询到数据
    /// </summary>
    public bool AutoUseWriteConnectionStringAfterWriteDb { get; set; }
    /// <summary>
    /// 当查询遇到没有路由被命中时是否抛出错误
    /// </summary>
    public bool ThrowIfQueryRouteNotMatch { get; set; }

    /// <summary>
    /// 全局配置最大的查询连接数限制,默认系统逻辑处理器<code>Environment.ProcessorCount</code>
    /// </summary>
    public int MaxQueryConnectionsLimit { get; set; }
    /// <summary>
    /// 默认<code>ConnectionModeEnum.SYSTEM_AUTO</code>
    /// </summary>
    public ConnectionModeEnum ConnectionMode { get; set; }
}