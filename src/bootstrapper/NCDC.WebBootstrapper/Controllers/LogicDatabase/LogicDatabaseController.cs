using Microsoft.AspNetCore.Mvc;
using NCDC.EntityFrameworkCore;
using NCDC.EntityFrameworkCore.Entities;
using NCDC.WebBootstrapper.Controllers.HttpParams;
using NCDC.WebBootstrapper.Controllers.LogicDatabase.Page;
using NCDC.WebBootstrapper.Extensions;

namespace NCDC.WebBootstrapper.Controllers.LogicDatabase;

[ApiController]
[Route("/api/logicDatabase")]
public class LogicDatabaseController : BaseApiController
{
    private readonly NCDCDbContext _ncdcDbContext;

    public LogicDatabaseController(NCDCDbContext ncdcDbContext)
    {
        _ncdcDbContext = ncdcDbContext;
    }

    [HttpGet,Route("page")]
    public AppResult<PagedResult<LogicDatabasePageResponse>> Page([FromQuery]LogicDatabasePageRequest request)
    {
        var page = _ncdcDbContext.Set<LogicDatabaseEntity>()
            .WhereIf(!string.IsNullOrWhiteSpace(request.DatabaseName),
                o => o.DatabaseName.Contains(request.DatabaseName!))
            .Select(o => new LogicDatabasePageResponse()
            {
                Id = o.Id,
                CreateTime = o.CreateTime,
                UpdateTime = o.UpdateTime,
                DatabaseName = o.DatabaseName,
                AutoUseWriteConnectionStringAfterWriteDb = o.AutoUseWriteConnectionStringAfterWriteDb,
                ThrowIfQueryRouteNotMatch = o.ThrowIfQueryRouteNotMatch,
                MaxQueryConnectionsLimit = o.MaxQueryConnectionsLimit,
                ConnectionMode = o.ConnectionMode
            }).ToPagedResult(request.Page, request.PageSize);
        return OutputOk(page);
    }
}