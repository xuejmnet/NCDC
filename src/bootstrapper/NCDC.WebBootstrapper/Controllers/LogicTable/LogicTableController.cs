using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NCDC.EntityFrameworkCore;
using NCDC.EntityFrameworkCore.Entities;
using NCDC.WebBootstrapper.Controllers.HttpParams;
using NCDC.WebBootstrapper.Controllers.LogicTable.ActualTableCreate;
using NCDC.WebBootstrapper.Controllers.LogicTable.ActualTablePage;
using NCDC.WebBootstrapper.Controllers.LogicTable.Create;
using NCDC.WebBootstrapper.Controllers.LogicTable.Page;
using NCDC.WebBootstrapper.Controllers.LogicTable.Update;
using NCDC.WebBootstrapper.Extensions;

namespace NCDC.WebBootstrapper.Controllers.LogicTable;

[ApiController]
[Route("/api/logic-table")]
public class LogicTableController : BaseApiController
{
    private readonly NCDCDbContext _ncdcDbContext;

    public LogicTableController(NCDCDbContext ncdcDbContext)
    {
        _ncdcDbContext = ncdcDbContext;
    }

    [HttpGet, Route("page")]
    public async Task<AppResult<PagedResult<LogicTablePageResponse>>> Page(
        [FromQuery] LogicTablePageRequest request)
    {
        var list = await _ncdcDbContext.Set<LogicTableEntity>()
            .Where(o => o.LogicDatabaseId == request.LogicDatabaseName!)
            .WhereIf(!string.IsNullOrWhiteSpace(request.TableName),o=>o.TableName.Contains(request.TableName!))
            .ProjectToType<LogicTablePageResponse>()
            .ToPagedResultAsync(request.Page, request.PageSize);
        return OutputOk(list);
    }
    [HttpPost, Route("create")]
    public async Task<AppResult<object>> Create(LogicTableCreateRequest request)
    {
        var logicDatabaseExists = await _ncdcDbContext.Set<LogicDatabaseEntity>()
            .AnyAsync(o => o.DatabaseName == request.LogicDatabaseName);
        if (!logicDatabaseExists)
        {
            return OutputFail($"不存在对应的逻辑数据库:[{request.LogicDatabaseName}]");
        }


        var hasTable = await _ncdcDbContext.Set<LogicTableEntity>()
            .AnyAsync(o =>o.LogicDatabaseId==request.LogicDatabaseName&& o.TableName == request.TableName);
        if (hasTable)
        {
            return OutputFail($"已存在逻辑表:[{request.TableName}]");
        }

        var logicTable = request.Adapt<LogicTableEntity>();
        logicTable.Id = Guid.NewGuid().ToString("n");
        await _ncdcDbContext.AddAsync(logicTable);
        await _ncdcDbContext.SaveChangesAsync();
        return OutputOk();
    }

    [HttpPost, Route("update")]
    public async Task<AppResult<object>> Update(LogicTableUpdateRequest request)
    {
        var logicTable =
            await _ncdcDbContext.Set<LogicTableEntity>().FirstOrDefaultAsync(o => o.Id == request.Id);
        if (logicTable == null)
        {
            return OutputFail($"未找到需要修改的逻辑表");
        }

        request.Adapt(logicTable);
        _ncdcDbContext.Update(logicTable);
        await _ncdcDbContext.SaveChangesAsync();
        return OutputOk();
    }

    [HttpDelete, Route("delete/{id}")]
    public async Task<AppResult<object>> Delete(string id)
    {
        var logicTable =
            await _ncdcDbContext.Set<LogicTableEntity>().FirstOrDefaultAsync(o => o.Id ==id);
        if (logicTable == null)
        {
            return OutputFail($"未找到需要修改的逻辑表");
        }

        await using (var tran = await _ncdcDbContext.Database.BeginTransactionAsync())
        {
            await _ncdcDbContext.Set<LogicTableEntity>().Where(o => o.Id == id)
                .ExecuteUpdateAsync(o => o.SetProperty(x => x.IsDelete, true));
            await _ncdcDbContext.Set<ActualTableEntity>().Where(o => o.LogicTableId == logicTable.TableName&&o.LogicDatabaseId==logicTable.LogicDatabaseId)
                .ExecuteUpdateAsync(o => o.SetProperty(x => x.IsDelete, true));
            await tran.CommitAsync();
        }
        return OutputOk();
    }

    [HttpGet, Route("actual-table-page")]
    public async Task<AppResult<PagedResult<ActualTablePageResponse>>> ActualTablePage(
        [FromQuery] ActualTablePageRequest request)
    {
        var list = await _ncdcDbContext.Set<ActualTableEntity>()
            .Where(o => o.LogicDatabaseId == request.LogicDatabaseName&&o.LogicTableId==request.LogicTableName)
            .ProjectToType<ActualTablePageResponse>()
            .ToPagedResultAsync(request.Page, request.PageSize);
        return OutputOk(list);
    }
    [HttpPost, Route("actual-table-create")]
    public async Task<AppResult<object>> ActualTableCreate(ActualTableCreateRequest request)
    {
        var logicDatabaseExists = await _ncdcDbContext.Set<LogicDatabaseEntity>()
            .AnyAsync(o => o.DatabaseName == request.LogicDatabaseName);
        if (!logicDatabaseExists)
        {
            return OutputFail($"不存在对应的逻辑数据库:[{request.LogicDatabaseName}]");
        }

        var hasDataSource = await _ncdcDbContext.Set<ActualDatabaseEntity>().AnyAsync(o=>o.LogicDatabaseId==request.LogicDatabaseName&&o.DataSourceName==request.DataSource);
        if (!hasDataSource)
        {
            return OutputFail($"当前数据源:[{request.DataSource}]不存在");
        }

        var hasTable = await _ncdcDbContext.Set<LogicTableEntity>()
            .AnyAsync(o =>o.LogicDatabaseId==request.LogicDatabaseName&& o.TableName == request.LogicTableName);
        if (!hasTable)
        {
            return OutputFail($"不存在逻辑表:[{request.LogicTableName}]");
        }
        var hasActualTable = await _ncdcDbContext.Set<ActualTableEntity>()
            .AnyAsync(o =>o.LogicDatabaseId==request.LogicDatabaseName&& o.LogicTableId == request.LogicTableName&&o.DataSource==request.DataSource&&o.TableName==request.TableName);
        if (hasActualTable)
        {
            return OutputFail($"当前数据源:[{request.DataSource}]已存在实际表:[{request.TableName}]");
        }

        var logicTable = request.Adapt<ActualTableEntity>();
        logicTable.Id = Guid.NewGuid().ToString("n");
        await _ncdcDbContext.AddAsync(logicTable);
        await _ncdcDbContext.SaveChangesAsync();
        return OutputOk();
    }

}