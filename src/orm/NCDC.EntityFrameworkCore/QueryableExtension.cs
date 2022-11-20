using Microsoft.EntityFrameworkCore;
using NCDC.EntityFrameworkCore.Entities.Base;

namespace NCDC.EntityFrameworkCore;
public static class QueryableExtension
{
    public static Task<int> LogicDeleteAsync<T>(this IQueryable<T> queryable) where T:ILogicDelete
    {
        return queryable.ExecuteUpdateAsync(o => o.SetProperty(p => p.IsDelete, p => true));
    }
}
