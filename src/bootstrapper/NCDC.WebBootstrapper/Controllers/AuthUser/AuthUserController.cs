using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NCDC.EntityFrameworkCore;
using NCDC.EntityFrameworkCore.Entities;
using NCDC.WebBootstrapper.Controllers.AuthUser.Create;
using NCDC.WebBootstrapper.Controllers.AuthUser.Page;
using NCDC.WebBootstrapper.Controllers.AuthUser.Update;
using NCDC.WebBootstrapper.Controllers.AuthUser.UserDatabases;
using NCDC.WebBootstrapper.Controllers.AuthUser.UserDatabasesSave;
using NCDC.WebBootstrapper.Controllers.HttpParams;
using NCDC.WebBootstrapper.Exceptions;
using NCDC.WebBootstrapper.Extensions;

namespace NCDC.WebBootstrapper.Controllers.AuthUser;

[ApiController]
[Route("/api/auth-user")]
public class AuthUserController : BaseApiController
{
    private readonly NCDCDbContext _ncdcDbContext;

    public AuthUserController(NCDCDbContext ncdcDbContext)
    {
        _ncdcDbContext = ncdcDbContext;
    }
    

    [HttpGet, Route("user-databases")]
    public async Task<AppResult<UserDatabasesResponse>> UserDatabases([FromQuery]UserDatabasesRequest request)
    {
        var result = new UserDatabasesResponse();
        
        var list = await _ncdcDbContext.Set<LogicDatabaseEntity>()
            .ProjectToType<UserDatabaseAllResponse>()
            .ToListAsync();
        result.AllDatabases.AddRange(list);
        
        var databases = await _ncdcDbContext.Set<DatabaseUserEntity>().Where(o=>o.AppAuthUserId==request.Id)
            .Select(o=>o.DatabaseId)
            .ToListAsync();
        result.CheckedDatabases.AddRange(databases);
        return OutputOk(result);
    }

    [HttpPost, Route("user-databases-save")]
    public async Task<AppResult<object>> UserDatabasesSave(UserDatabasesSaveRequest request)
    {
        var hasUser = await _ncdcDbContext.Set<AppAuthUserEntity>().AnyAsync(o => o.Id == request.Id);
        if (!hasUser)
        {
            throw new AppException("未找到当前用户");
        }

        var dbDatabaseUsers = await _ncdcDbContext.Set<DatabaseUserEntity>().Where(o=>o.AppAuthUserId==request.Id).ToListAsync();
        var deleteDatabaseIds = dbDatabaseUsers.Select(o=>o.DatabaseId).Except(request.CheckedDatabases).ToList();
        var insertDatabaseIds = request.CheckedDatabases.Except(dbDatabaseUsers.Select(o=>o.DatabaseId)).ToList();
        var insertDatabaseUsers = insertDatabaseIds.Select(o =>
        {
            var databaseUser = new DatabaseUserEntity();
            databaseUser.Id = Guid.NewGuid().ToString("n");
            databaseUser.DatabaseId = o;
            databaseUser.AppAuthUserId = request.Id;
            return databaseUser;
        }).ToList();
        await using (var tran = await _ncdcDbContext.Database.BeginTransactionAsync())
        {
            await _ncdcDbContext.AddRangeAsync(insertDatabaseUsers);
            await _ncdcDbContext.Set<DatabaseUserEntity>().Where(o=>o.AppAuthUserId==request.Id&&deleteDatabaseIds.Contains(o.DatabaseId))
                .ExecuteUpdateAsync(o => o.SetProperty(x => x.IsDelete, true));
            await _ncdcDbContext.SaveChangesAsync();
            await tran.CommitAsync();
        }
        return OutputOk();
    }
    
    [HttpGet, Route("page")]
    public async Task<AppResult<PagedResult<AuthUserPageResponse>>> Page(
        [FromQuery] AuthUserPageRequest request)
    {
        var list = await _ncdcDbContext.Set<AppAuthUserEntity>()
            .WhereIf(!string.IsNullOrWhiteSpace(request.UserName), o => o.UserName.Contains(request.UserName!))
            .ProjectToType<AuthUserPageResponse>()
            .ToPagedResultAsync(request.Page, request.PageSize);
        return OutputOk(list);
    }

    //todo 新增 修改 删除 获取所关联的数据库
    [HttpPost, Route("create")]
    public async Task<AppResult<object>> Create(AuthUserCreateRequest request)
    {
        var hasUser = await _ncdcDbContext.Set<AppAuthUserEntity>()
            .AnyAsync(o => o.UserName == request.UserName);
        if (hasUser)
        {
            return OutputFail($"用户名:[{request.UserName}]已存在");
        }

        var actualDatabase = request.Adapt<AppAuthUserEntity>();
        actualDatabase.Id = Guid.NewGuid().ToString("n");
        await _ncdcDbContext.AddAsync(actualDatabase);
        await _ncdcDbContext.SaveChangesAsync();
        return OutputOk();
    }

    [HttpPost, Route("update")]
    public async Task<AppResult<object>> Update(AuthUserUpdateRequest request)
    {
        var appUser = await _ncdcDbContext.Set<AppAuthUserEntity>().FirstOrDefaultAsync(o => o.Id == request.Id);
        if (appUser == null)
        {
            return OutputFail($"未找到需要修改的用户");
        }
        request.Adapt(appUser);
        _ncdcDbContext.Update(appUser);
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
        var appUser = await _ncdcDbContext.Set<AppAuthUserEntity>().FirstOrDefaultAsync(o => o.Id == id);
        if (appUser == null)
        {
            return OutputFail($"未找到需要修改的用户");
        }

        await using (var tran = await _ncdcDbContext.Database.BeginTransactionAsync())
        {
            await _ncdcDbContext.Set<AppAuthUserEntity>().Where(o => o.Id == id)
                .ExecuteUpdateAsync(o => o.SetProperty(x => x.IsDelete, true));
            await _ncdcDbContext.Set<DatabaseUserEntity>().Where(o => o.AppAuthUserId == id)
                .ExecuteUpdateAsync(o => o.SetProperty(x => x.IsDelete, true));
            await tran.CommitAsync();
        }
        return OutputOk();
    }
}