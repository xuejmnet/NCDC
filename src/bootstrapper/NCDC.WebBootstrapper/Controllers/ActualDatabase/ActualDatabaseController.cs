using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NCDC.EntityFrameworkCore;
using NCDC.EntityFrameworkCore.Entities;
using NCDC.WebBootstrapper.Controllers.ActualDatabase.All;
using NCDC.WebBootstrapper.Controllers.ActualDatabase.Create;
using NCDC.WebBootstrapper.Controllers.ActualDatabase.Page;
using NCDC.WebBootstrapper.Controllers.ActualDatabase.Update;
using NCDC.WebBootstrapper.Controllers.HttpParams;
using NCDC.WebBootstrapper.Extensions;

namespace NCDC.WebBootstrapper.Controllers.ActualDatabase;

[ApiController]
[Route("/api/actual-database")]
public class ActualDatabaseController : BaseApiController
{
    private readonly NCDCDbContext _ncdcDbContext;

    public ActualDatabaseController(NCDCDbContext ncdcDbContext)
    {
        _ncdcDbContext = ncdcDbContext;
    }

    [HttpGet, Route("page")]
    public async Task<AppResult<PagedResult<ActualDatabasePageResponse>>> Page(
        [FromQuery] ActualDatabasePageRequest request)
    {
        var list = await _ncdcDbContext.Set<ActualDatabaseEntity>()
            .Where(o => o.LogicDatabaseId == request.LogicDatabaseId!)
            .WhereIf(!string.IsNullOrWhiteSpace(request.DataSourceName),o=>o.DataSourceName.Contains(request.DataSourceName!))
            .ProjectToType<ActualDatabasePageResponse>()
            .ToPagedResultAsync(request.Page, request.PageSize);
        return OutputOk(list);
    }

    [HttpPost, Route("create")]
    public async Task<AppResult<object>> Create(ActualDatabaseCreateRequest request)
    {
        var logicDatabase = await _ncdcDbContext.Set<LogicDatabaseEntity>()
            .FirstOrDefaultAsync(o => o.Id == request.LogicDatabaseId);
        if (logicDatabase==null)
        {
            return OutputFail($"不存在对应的逻辑数据库");
        }

        if (request.IsDefault)
        {
            var hasDefault = await _ncdcDbContext.Set<ActualDatabaseEntity>().AnyAsync(o =>o.LogicDatabaseId==logicDatabase.Id&& o.IsDefault);
            if (hasDefault)
            {
                return OutputFail("已存在默认数据源");
            }
        }
        else
        {
            var hasDefault = await _ncdcDbContext.Set<ActualDatabaseEntity>().AnyAsync(o =>o.LogicDatabaseId==logicDatabase.Id&& o.IsDefault);
            if (!hasDefault)
            {
                return OutputFail("当前数据源不存在默认数据源,请先添加默认数据源");
            }
        }

        var hasDataSource = await _ncdcDbContext.Set<ActualDatabaseEntity>()
            .AnyAsync(o =>o.LogicDatabaseId==logicDatabase.Id&& o.DataSourceName == request.DataSourceName);
        if (hasDataSource)
        {
            return OutputFail($"已存在数据源:[{request.DataSourceName}]");
        }

        var actualDatabase = request.Adapt<ActualDatabaseEntity>();
        actualDatabase.Id = Guid.NewGuid().ToString("n");
        await _ncdcDbContext.AddAsync(actualDatabase);
        await _ncdcDbContext.SaveChangesAsync();
        return OutputOk();
    }

    [HttpPost, Route("update")]
    public async Task<AppResult<object>> Update(ActualDatabaseUpdateRequest request)
    {

        var actualDatabase =
            await _ncdcDbContext.Set<ActualDatabaseEntity>().FirstOrDefaultAsync(o => o.Id == request.Id);
        if (actualDatabase == null)
        {
            return OutputFail($"未找到需要修改的数据库");
        }

        var hasDataSource = await _ncdcDbContext.Set<ActualDatabaseEntity>()
            .AnyAsync(o =>o.LogicDatabaseId==actualDatabase.LogicDatabaseId&&  o.DataSourceName == request.DataSourceName && o.Id != request.Id);
        if (hasDataSource)
        {
            return OutputFail($"已存在数据源:[{request.DataSourceName}]");
        }

        request.Adapt(actualDatabase);
        _ncdcDbContext.Update(actualDatabase);
        await _ncdcDbContext.SaveChangesAsync();
        return OutputOk();
    }

    [HttpDelete, Route("delete/{id}")]
    public async Task<AppResult<object>> Delete(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return OutputFail($"请选择需要删除的记录");
        }
        var actualDatabase =
            await _ncdcDbContext.Set<ActualDatabaseEntity>().FirstOrDefaultAsync(o => o.Id == id);
        if (actualDatabase == null)
        {
            return OutputFail($"未找到需要修改的数据库");
        }

        if (actualDatabase.IsDefault)
        {
            var hasOther = await _ncdcDbContext.Set<ActualDatabaseEntity>().AnyAsync(o => o.Id != id);
            if (hasOther)
            {
                return OutputFail($"默认数据库只能最后删除");
            }
        }

        var hasActualTable =await _ncdcDbContext.Set<ActualTableEntity>().AnyAsync(o=>o.DataSourceId==actualDatabase.Id);

        if (hasActualTable)
        {
            return OutputFail($"当前数据源存在对应的实际表无法删除");
        }
        await _ncdcDbContext.Set<ActualDatabaseEntity>().Where(o => o.Id == id)
            .ExecuteUpdateAsync(o => o.SetProperty(x => x.IsDelete, true));
        return OutputOk();
    }
    [HttpGet, Route("all")]
    public async Task<AppResult<List<ActualDatabaseAllResponse>>> All(
        [FromQuery] ActualDatabaseAllRequest request)
    {
        var list = await _ncdcDbContext.Set<ActualDatabaseEntity>()
            .Where(o => o.LogicDatabaseId == request.LogicDatabaseId)
            .ProjectToType<ActualDatabaseAllResponse>()
            .ToListAsync();
        return OutputOk(list);
    }
}