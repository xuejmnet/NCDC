namespace NCDC.WebBootstrapper.Controllers.HttpParams;

public class PagedResult<T>
{

    #region Ctor
    /// <summary>
    /// 初始化一个新的<c>PagedResult{T}</c>类型的实例。
    /// </summary>
    /// <param name="total">总记录数。</param>
    /// <param name="items">当前页面的数据。</param>
    public PagedResult(List<T> items, int total)
    {
        this.Total = total;
        this.Items = items;
    }
    #endregion

    #region Public Properties
    /// <summary>
    /// 获取或设置总记录数。
    /// </summary>
    public int Total { get; set; }
    /// <summary>
    /// 分页数据
    /// </summary>
    public List<T> Items { get; set; }
    #endregion
}