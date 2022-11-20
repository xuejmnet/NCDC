
using Newtonsoft.Json;

namespace NCDC.WebBootstrapper.Controllers.ActualDatabase.Page;

public class ActualDatabasePageResponse
{
    
    public string Id { get; set; } = null!;
    public DateTime CreateTime { get; set; }
    public DateTime UpdateTime { get; set; }
    /// <summary>
    /// 所属逻辑数据库名称
    /// </summary>
    public string LogicDatabaseName { get; set; } = null!;
    /// <summary>
    /// 数据源名称
    /// </summary>
    public string DataSourceName { get; set; } = null!;
    /// <summary>
    /// 数据源链接
    /// </summary>
    // [JsonIgnore]
    public string ConnectionString { get; set; } = null!;
    // [JsonProperty("connectionString")]
    // public string NewConnectionString
    // {
    //     get
    //     {
    //         if (!string.IsNullOrWhiteSpace(ConnectionString)&&ConnectionString.Contains("password",StringComparison.OrdinalIgnoreCase))
    //         {
    //             var conItems = ConnectionString.Split(";",StringSplitOptions.RemoveEmptyEntries);
    //             var list = new List<string>(conItems.Length);
    //             foreach (var s in conItems)
    //             {
    //                 if (s.Trim().IndexOf("password", StringComparison.OrdinalIgnoreCase)==0)
    //                 {
    //                     var indexOf = s.IndexOf("=",StringComparison.OrdinalIgnoreCase);
    //                     if (indexOf >= 0)
    //                     {
    //                         var password = s.Substring(0,indexOf+1);
    //                         list.Add(password+";");
    //                     }
    //                     else
    //                     {
    //                         list.Add(s);
    //                     }
    //                 }
    //                 else
    //                 {
    //                     list.Add(s);
    //                 }
    //             }
    //
    //             return string.Join(";", list);
    //         }
    //
    //         return ConnectionString;
    //     }
    // }

    /// <summary>
    /// 是否默认数据源
    /// </summary>
    public bool IsDefault { get; set; }
}