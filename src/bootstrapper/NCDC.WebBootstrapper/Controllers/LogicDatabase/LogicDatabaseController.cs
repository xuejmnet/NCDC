using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NCDC.EntityFrameworkCore;
using NCDC.EntityFrameworkCore.Entities;
using NCDC.WebBootstrapper.Controllers.HttpParams;
using NCDC.WebBootstrapper.Controllers.LogicDatabase.All;
using NCDC.WebBootstrapper.Controllers.LogicDatabase.Create;
using NCDC.WebBootstrapper.Controllers.LogicDatabase.Page;
using NCDC.WebBootstrapper.Controllers.LogicDatabase.Update;
using NCDC.WebBootstrapper.Extensions;

namespace NCDC.WebBootstrapper.Controllers.LogicDatabase;

[ApiController]
[Route("/api/logic-database")]
public class LogicDatabaseController : BaseApiController
{
    private readonly NCDCDbContext _ncdcDbContext;

    public LogicDatabaseController(NCDCDbContext ncdcDbContext)
    {
        _ncdcDbContext = ncdcDbContext;
    }

    [HttpGet,Route("page")]
    public async Task<AppResult<PagedResult<LogicDatabasePageResponse>>> Page([FromQuery]LogicDatabasePageRequest request)
    {
        var page =await _ncdcDbContext.Set<LogicDatabaseEntity>()
            .WhereIf(!string.IsNullOrWhiteSpace(request.DatabaseName),
                o => o.DatabaseName.Contains(request.DatabaseName!))
            .ProjectToType<LogicDatabasePageResponse>().OrderByDescending(o=>o.CreateTime).ToPagedResultAsync(request.Page, request.PageSize);
        return OutputOk(page);
    }
    [HttpPost,Route("create")]
    public async Task<AppResult<object>> Create(LogicDatabaseCreateRequest request)
    {
        var exists = await _ncdcDbContext.Set<LogicDatabaseEntity>()
            .AnyAsync(o => o.DatabaseName == request.DatabaseName);
        if (exists)
        {
            return OutputFail($"数据库名称:[{request.DatabaseName}]已存在");
        }
        
        var logicDatabase =request.Adapt<LogicDatabaseEntity>();
        logicDatabase.Id = Guid.NewGuid().ToString("n");
        await _ncdcDbContext.AddAsync(logicDatabase);
        await _ncdcDbContext.SaveChangesAsync();
        return OutputOk();
    }
    [HttpPost,Route("update")]
    public async Task<AppResult<object>> Update(LogicDatabaseUpdateRequest request)
    {
        var logicDatabase = await _ncdcDbContext.Set<LogicDatabaseEntity>()
            .FirstOrDefaultAsync(o => o.Id == request.Id);
        if (logicDatabase == null)
        {
            return OutputFail("未找到需要修改的数据库");
        }
        
        var exists = await _ncdcDbContext.Set<LogicDatabaseEntity>()
            .AnyAsync(o => o.DatabaseName == request.DatabaseName&&o.Id!=logicDatabase.Id);
        if (exists)
        {
            return OutputFail($"数据库名称:[{request.DatabaseName}]已存在");
        }
     
        request.Adapt(logicDatabase);
        _ncdcDbContext.Update(logicDatabase);
        await _ncdcDbContext.SaveChangesAsync();
        return OutputOk();
    }
    [HttpDelete,Route("delete/{id}")]
    public async Task<AppResult<object>> Delete(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return OutputFail("请求id不能为空");
        
        var logicDatabase = await _ncdcDbContext.Set<LogicDatabaseEntity>()
            .FirstOrDefaultAsync(o => o.Id == id);
        if (logicDatabase==null)
        {
            return OutputFail("请求数据库不存在");
        }

        await using (var tran = await _ncdcDbContext.Database.BeginTransactionAsync())
        {
            await _ncdcDbContext.Set<LogicDatabaseEntity>().Where(o => o.Id == id).ExecuteUpdateAsync(o => o.SetProperty(p => p.IsDelete, p => true));
            await _ncdcDbContext.Set<ActualDatabaseEntity>().Where(o => o.LogicDatabaseId == logicDatabase.Id).ExecuteUpdateAsync(o => o.SetProperty(p => p.IsDelete, p => true));
            await _ncdcDbContext.Set<LogicTableEntity>().Where(o => o.LogicDatabaseId == logicDatabase.Id).ExecuteUpdateAsync(o => o.SetProperty(p => p.IsDelete, p => true));
            await _ncdcDbContext.Set<ActualTableEntity>().Where(o => o.LogicDatabaseId == logicDatabase.Id).ExecuteUpdateAsync(o => o.SetProperty(p => p.IsDelete, p => true));
            await _ncdcDbContext.Set<DatabaseUserEntity>().Where(o => o.DatabaseId == logicDatabase.Id).ExecuteUpdateAsync(o => o.SetProperty(p => p.IsDelete, p => true));
            
            await tran.CommitAsync();
        }
        return OutputOk();
    }

    [HttpGet, Route("all")]
    public async Task<AppResult<List<LogicDatabaseAllResponse>>> All()
    {
        var list = await _ncdcDbContext.Set<LogicDatabaseEntity>().ProjectToType<LogicDatabaseAllResponse>().ToListAsync();
        return OutputOk(list);
    }
}