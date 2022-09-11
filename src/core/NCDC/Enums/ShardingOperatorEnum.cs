using System.ComponentModel;

namespace NCDC.Enums;



/// <summary>
/// 分片条件比较符
/// </summary>
public enum ShardingOperatorEnum
{
    /// <summary>
    /// 未知操作符
    /// </summary>
    [Description("??")] UN_KNOWN,

    /// <summary>
    /// 大于
    /// </summary>
    [Description(">")] GREATER_THAN,

    /// <summary>
    /// 大于等于
    /// </summary>
    [Description(">=")] GREATER_THAN_OR_EQUAL,

    /// <summary>
    /// 小于
    /// </summary>
    [Description("<")] LESS_THAN,

    /// <summary>
    /// 小于等于
    /// </summary>
    [Description("<=")] LESS_THAN_OR_EQUAL,

    /// <summary>
    /// 等于
    /// </summary>
    [Description("==")] EQUAL,

    /// <summary>
    /// 不等于
    /// </summary>
    [Description("!=")] NOT_EQUAL,

    /// <summary>
    /// like 类似 contains
    /// </summary>
    [Description("%w%")] ALL_LIKE,

    /// <summary>
    /// like 类似 start with
    /// </summary>
    [Description("w%")] START_LIKE,

    /// <summary>
    /// like 类似 end with
    /// </summary>
    [Description("%w")] END_LIKE
}