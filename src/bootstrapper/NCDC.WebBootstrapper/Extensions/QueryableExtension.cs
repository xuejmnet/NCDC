using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NCDC.WebBootstrapper.Controllers.HttpParams;

namespace NCDC.WebBootstrapper.Extensions;

public static class QueryableExtension
{
    public static IQueryable<T> WhereIf<T>(
        this IQueryable<T> source,
        bool condition,
        Expression<Func<T, bool>> predicate)
    {
        return !condition ? source : source.Where<T>(predicate);
    }


    public static PagedResult<T> ToPagedResult<T>(this IQueryable<T> source, int pageIndex, int pageSize)
    {
        //设置每次获取多少页
        var take = pageSize <= 0 ? 1 : pageSize;
        //设置当前页码最小1
        var index = pageIndex <= 0 ? 1 : pageIndex;
        //需要跳过多少页
        var skip = (index - 1) * take;

        //获取每次总记录数
        var count = source.Count();
        if (count <= skip)
            return new PagedResult<T>(new List<T>(0), count);
        //获取剩余条数
        var remainingCount = count - skip;
        //当剩余条数小于take数就取remainingCount
        var realTake = remainingCount < take ? remainingCount : take;
        var data = source.Skip(skip).Take(realTake).ToList();
        return new PagedResult<T>(data, count);
    }
}