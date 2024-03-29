using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NCDC.ShardingTest.Seed;
using NCDC.ShardingTest.Seed.Entities;
using NCDC.ShardingTest.Seed.Shardings;
using NCDC.ShardingTest.Seed2;
using NCDC.ShardingTest.Seed2.Entities;
using ShardingCore;
using ShardingCore.Helpers;

namespace NCDC.ShardingTest;

public class Startup
{
    public void ConfigureServices(IServiceCollection services, HostBuilderContext hostBuilderContext)
    {
        services.AddDbContext<TypeSeedDbContext>(o => o.UseMySql("server=127.0.0.1;port=3306;database=ncdctype;userid=root;password=root;", new MySqlServerVersion(new Version())));
        services.AddShardingDbContext<ShardingDefaultDbContext>()
            .UseRouteConfig(op =>
            {
                op.AddShardingDataSourceRoute<OrderAreaShardingVirtualDataSourceRoute>();
                op.AddShardingTableRoute<SysUserModVirtualTableRoute>();
                op.AddShardingTableRoute<SysUserSalaryVirtualTableRoute>();
                op.AddShardingTableRoute<OrderCreateTimeVirtualTableRoute>();
                op.AddShardingTableRoute<LogDayVirtualTableRoute>();
                op.AddShardingTableRoute<LogWeekDateTimeVirtualTableRoute>();
                op.AddShardingTableRoute<LogWeekTimeLongVirtualTableRoute>();
                op.AddShardingTableRoute<LogYearDateTimeVirtualRoute>();
                op.AddShardingTableRoute<LogMonthLongvirtualRoute>();
                op.AddShardingTableRoute<LogYearLongVirtualRoute>();
                op.AddShardingTableRoute<SysUserModIntVirtualRoute>();
                op.AddShardingTableRoute<LogDayLongVirtualRoute>();
                op.AddShardingTableRoute<MultiShardingOrderVirtualTableRoute>();
            })
            .UseConfig(op =>
            {
                //op.UseEntityFrameworkCoreProxies = true;
                //当无法获取路由时会返回默认值而不是报错
                op.ThrowIfQueryRouteNotMatch = false;
                //忽略建表错误compensate table和table creator
                op.IgnoreCreateTableError = true;
                //迁移时使用的并行线程数(分库有效)defaultShardingDbContext.Database.Migrate()
                op.MigrationParallelCount = Environment.ProcessorCount;
                //补偿表创建并行线程数 调用UseAutoTryCompensateTable有效
                op.CompensateTableParallelCount = Environment.ProcessorCount;
                //最大连接数限制
                op.MaxQueryConnectionsLimit = Environment.ProcessorCount;
                //链接模式系统默认
                //如何通过字符串查询创建DbContext
                op.UseShardingQuery((conStr, builder) =>
                {
                    builder.UseMySql(conStr, new MySqlServerVersion(new Version()));
                });
                //如何通过事务创建DbContext
                op.UseShardingTransaction((connection, builder) =>
                {
                    builder.UseMySql(connection, new MySqlServerVersion(new Version()));
                });
                //添加默认数据源
                op.AddDefaultDataSource("A",
                    "server=127.0.0.1;port=3306;database=ncdc1;userid=root;password=root;");
                //添加额外数据源
                op.AddExtraDataSource(sp =>
                {
                    return new Dictionary<string, string>()
                    {
                        { "B", "server=127.0.0.1;port=3306;database=ncdc2;userid=root;password=root;" },
                        { "C", "server=127.0.0.1;port=3306;database=ncdc3;userid=root;password=root;" },
                    };
                });
            })
            .AddShardingCore();
    }

    public void Configure(IServiceProvider serviceProvider)
    {
        //启动ShardingCore创建表任务
        //启动进行表补偿
        serviceProvider.UseAutoTryCompensateTable();
        // 有一些测试数据要初始化可以放在这里
        InitData(serviceProvider).GetAwaiter().GetResult();
    }

    /// <summary>
    /// 添加种子数据
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    private async Task InitData(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var typeSeedDbContext = scope.ServiceProvider.GetRequiredService<TypeSeedDbContext>();
            await typeSeedDbContext.Database.EnsureCreatedAsync();
            if (!await typeSeedDbContext.Set<StringEntity>().AnyAsync())
            {
                var stringEntities = new List<StringEntity>(10000);
                for (int i = 0; i < 10000; i++)
                {
                    var stringEntity = new StringEntity();
                    stringEntity.Id = $"{i}";
                    stringEntity.Column1 = Guid.NewGuid().ToString("n");
                    stringEntity.Column2 = i%2==0?Guid.NewGuid().ToString("n"):null;
                    stringEntity.Column3 = Guid.NewGuid().ToString("n");
                    stringEntity.Column4 = i%2==0?Guid.NewGuid().ToString("n"):null;
                    stringEntity.Column5 = Guid.NewGuid().ToString("n");
                    stringEntity.Column6 = i%2==0?Guid.NewGuid().ToString("n"):null;
                    stringEntity.Column7 = Guid.NewGuid().ToString("n");
                    stringEntity.Column8 = i%2==0?Guid.NewGuid().ToString("n"):null;
                    stringEntity.Column9 = Guid.NewGuid().ToString("n");
                    stringEntity.Column10 = i%2==0?Guid.NewGuid().ToString("n"):null;
                    stringEntity.Column11 = Guid.NewGuid().ToString("n");
                    stringEntity.Column12 = i%2==0?Guid.NewGuid().ToString("n"):null;
                    stringEntity.Column13 = Guid.NewGuid().ToString("n");
                    stringEntity.Column14 = i%2==0?Guid.NewGuid().ToString("n"):null;
                    stringEntity.Column15 = Guid.NewGuid().ToString("n");
                    stringEntity.Column16 = i%2==0?Guid.NewGuid().ToString("n"):null;
                    stringEntity.Column17 = Guid.NewGuid().ToString("n");
                    stringEntity.Column18 = i%2==0?Guid.NewGuid().ToString("n"):null;
                    stringEntity.Column19 = Guid.NewGuid().ToString("n");
                    stringEntity.Column20 = i%2==0?Guid.NewGuid().ToString("n"):null;
                    stringEntities.Add(stringEntity);
                }
                
                await typeSeedDbContext.AddRangeAsync(stringEntities);
                
                
                var numberEntities = new List<NumberEntity>(10000);
                for (int i = 0; i < 10000; i++)
                {
                    var numberEntity = new NumberEntity();
                    numberEntity.Id = $"{i}";
                    numberEntity.Column1 =i%2==0? (byte)1: (byte)0;
                    numberEntity.Column2 = i%2==0? (byte)1: (byte)0;
                    numberEntity.Column3 = i%2==0? (sbyte)1: (sbyte)0;
                    numberEntity.Column4 = i%2==0? (sbyte)1: (sbyte)0;
                    numberEntity.Column5 = (short)new Random().Next(Int16.MaxValue);
                    numberEntity.Column6 = i%2==0?(short)new Random().Next(Int16.MaxValue):null;
                    numberEntity.Column7 =(ushort)new Random().Next(Int16.MaxValue);
                    numberEntity.Column8 = i%2==0?(ushort)new Random().Next(Int16.MaxValue):null;
                    numberEntity.Column9 =new Random().Next(Int32.MaxValue);
                    numberEntity.Column10 = i%2==0?new Random().Next(Int32.MaxValue):null;
                    numberEntity.Column11 = (uint)new Random().Next(Int32.MaxValue);
                    numberEntity.Column12 = i%2==0?(uint)new Random().Next(Int32.MaxValue):null;
                    numberEntity.Column13 = new Random().NextInt64(Int64.MaxValue);
                    numberEntity.Column14 = i%2==0?new Random().NextInt64(Int64.MaxValue):null;
                    numberEntity.Column15 = (ulong)new Random().NextInt64(Int64.MaxValue);
                    numberEntity.Column16 = i%2==0?(ulong)new Random().NextInt64(Int64.MaxValue):null;
                    numberEntity.Column17 = (float)new Random().NextDouble();
                    numberEntity.Column18 =i%2==0?(float)new Random().NextDouble():null;
                    numberEntity.Column19 = new Random().NextDouble();
                    numberEntity.Column20 = i%2==0?new Random().NextDouble():null;
                    numberEntity.Column21 = (decimal)Math.Round((new Random().NextDouble() / 100),6);
                    numberEntity.Column22 = i%2==0?(decimal)Math.Round((new Random().NextDouble() / 100),6):null;
                    numberEntity.Column23 = i%2==0;
                    numberEntity.Column24 = i%2==0?true:null;
                   numberEntities.Add(numberEntity);
                }
                
                var dateTimeEntities = new List<DateTimeEntity>(10000);
                for (int i = 0; i < 10000; i++)
                {
                    var dateTimeEntity = new DateTimeEntity();
                    dateTimeEntity.Id = $"{i}";
                    dateTimeEntity.Column1 = DateTime.Now.AddMinutes(new Random().Next(1,999999)).Year;
                    dateTimeEntity.Column2 = i%2==0? DateTime.Now.AddMinutes(new Random().Next(1,999999)).Year: null;
                    dateTimeEntity.Column3 =DateTime.Now.AddMinutes(new Random().Next(1,999999));
                    dateTimeEntity.Column4 = i%2==0? DateTime.Now.AddMinutes(new Random().Next(1,999999)): null;
                    dateTimeEntity.Column5 = DateTime.Now.AddMinutes(new Random().Next(1,999999));
                    dateTimeEntity.Column6 = i%2==0? DateTime.Now.AddMinutes(new Random().Next(1,999999)): null;
                    dateTimeEntity.Column7 =DateTime.Now.AddMinutes(new Random().Next(1,999999));
                    dateTimeEntity.Column8 =  i%2==0? DateTime.Now.AddMinutes(new Random().Next(1,999999)): null;
                    var time1 = DateTime.Now.AddSeconds(new Random().Next(1,9999999));
                    dateTimeEntity.Column9 =new TimeOnly(time1.Hour, time1.Minute, time1.Second);
                    var time2 = DateTime.Now.AddSeconds(new Random().Next(1,9999999));
                    dateTimeEntity.Column10 =  i%2==0? new TimeOnly(time2.Hour, time2.Minute, time2.Second): null;
                   dateTimeEntities.Add(dateTimeEntity);
                }
                
                await typeSeedDbContext.AddRangeAsync(stringEntities);
                await typeSeedDbContext.AddRangeAsync(numberEntities);
                await typeSeedDbContext.AddRangeAsync(dateTimeEntities);
                
                await typeSeedDbContext.SaveChangesAsync();
            }
            var virtualDbContext = scope.ServiceProvider.GetRequiredService<ShardingDefaultDbContext>();
            if (!await virtualDbContext.Set<SysUserMod>().AnyAsync())
            {
                var ids = Enumerable.Range(1, 1000);
                var userMods = new List<SysUserMod>();
                var userModInts = new List<SysUserModInt>();
                var userSalaries = new List<SysUserSalary>();
                var beginTime = new DateTime(2020, 1, 1);
                var endTime = new DateTime(2021, 12, 1);
                foreach (var id in ids)
                {
                    userMods.Add(new SysUserMod()
                    {
                        Id = id.ToString(),
                        Age = id,
                        Name = $"name_{id}",
                        AgeGroup = Math.Abs(id % 10)
                    });
                    userModInts.Add(new SysUserModInt()
                    {
                        Id = id,
                        Age = id,
                        Name = $"name_{id}",
                        AgeGroup = Math.Abs(id % 10)
                    });
                    var tempTime = beginTime;
                    var i = 0;
                    while (tempTime <= endTime)
                    {
                        var dateOfMonth = $@"{tempTime:yyyyMM}";
                        userSalaries.Add(new SysUserSalary()
                        {
                            Id = $@"{id}{dateOfMonth}",
                            UserId = id.ToString(),
                            DateOfMonth = int.Parse(dateOfMonth),
                            Salary = 700000 + id * 100 * i,
                            SalaryLong = 700000 + id * 100 * i,
                            SalaryDecimal = (700000 + id * 100 * i) / 100m,
                            SalaryDouble = (700000 + id * 100 * i) / 100d,
                            SalaryFloat = (700000 + id * 100 * i) / 100f
                        });
                        tempTime = tempTime.AddMonths(1);
                        i++;
                    }
                }

                var areas = new List<string>() { "A", "B", "C" };
                List<Order> orders = new List<Order>(360);
                var begin = new DateTime(2021, 1, 1);
                for (int i = 0; i < 320; i++)
                {
                    orders.Add(new Order()
                    {
                        Id = Guid.NewGuid(),
                        Area = areas[i % 3],
                        CreateTime = begin,
                        Money = i
                    });
                    begin = begin.AddDays(1);
                }

                List<LogDay> logDays = new List<LogDay>(3600);
                List<LogDayLong> logDayLongs = new List<LogDayLong>(3600);

                var levels = new List<string>() { "info", "warning", "error" };
                var begin1 = new DateTime(2021, 1, 1);
                for (int i = 0; i < 300; i++)
                {
                    var ltime = begin1;
                    for (int j = 0; j < 10; j++)
                    {
                        logDays.Add(new LogDay()
                        {
                            Id = Guid.NewGuid(),
                            LogLevel = levels[j % 3],
                            LogBody = $"{i}_{j}",
                            LogTime = ltime.AddHours(1)
                        });
                        logDayLongs.Add(new LogDayLong()
                        {
                            Id = Guid.NewGuid(),
                            LogLevel = levels[j % 3],
                            LogBody = $"{i}_{j}",
                            LogTime = ShardingCoreHelper.ConvertDateTimeToLong(ltime.AddHours(1))
                        });
                        ltime = ltime.AddHours(1);
                    }

                    begin1 = begin1.AddDays(1);
                }

                List<LogWeekDateTime> logWeeks = new List<LogWeekDateTime>(300);
                var begin2 = new DateTime(2021, 1, 1);
                for (int i = 0; i < 300; i++)
                {
                    logWeeks.Add(new LogWeekDateTime()
                    {
                        Id = Guid.NewGuid().ToString("n"),
                        Body = $"body_{i}",
                        LogTime = begin2
                    });
                    begin2 = begin2.AddDays(1);
                }

                List<LogWeekTimeLong> logWeekLongs = new List<LogWeekTimeLong>(300);
                var begin3 = new DateTime(2021, 1, 1);
                for (int i = 0; i < 300; i++)
                {
                    logWeekLongs.Add(new LogWeekTimeLong()
                    {
                        Id = Guid.NewGuid().ToString("n"),
                        Body = $"body_{i}",
                        LogTime = ShardingCoreHelper.ConvertDateTimeToLong(begin3)
                    });
                    begin3 = begin3.AddDays(1);
                }

                List<LogYearDateTime> logYears = new List<LogYearDateTime>(600);
                var begin4 = new DateTime(2020, 1, 1);
                for (int i = 0; i < 600; i++)
                {
                    logYears.Add(new LogYearDateTime()
                    {
                        Id = Guid.NewGuid(),
                        LogBody = $"body_{i}",
                        LogTime = begin4
                    });
                    begin4 = begin4.AddDays(1);
                }


                List<LogMonthLong> logMonthLongs = new List<LogMonthLong>(300);
                var begin5 = new DateTime(2021, 1, 1);
                for (int i = 0; i < 300; i++)
                {
                    logMonthLongs.Add(new LogMonthLong()
                    {
                        Id = Guid.NewGuid().ToString("n"),
                        Body = $"body_{i}",
                        LogTime = ShardingCoreHelper.ConvertDateTimeToLong(begin5)
                    });
                    begin5 = begin5.AddDays(1);
                }

                List<LogYearLong> logYearkLongs = new List<LogYearLong>(300);
                var begin6 = new DateTime(2021, 1, 1);
                for (int i = 0; i < 300; i++)
                {
                    logYearkLongs.Add(new LogYearLong()
                    {
                        Id = Guid.NewGuid().ToString("n"),
                        LogBody = $"body_{i}",
                        LogTime = ShardingCoreHelper.ConvertDateTimeToLong(begin6)
                    });
                    begin6 = begin6.AddDays(1);
                }

                var multiShardingOrders = new List<MultiShardingOrder>(9);

                #region 添加多字段分表

                {
                    var now = new DateTime(2021, 10, 1, 13, 13, 11);
                    multiShardingOrders.Add(new MultiShardingOrder()
                    {
                        Id = 231765457240207360,
                        Name = $"{now:yyyy/MM/dd HH:mm:ss}",
                        CreateTime = now
                    });
                }
                {
                    var now = new DateTime(2021, 10, 2, 11, 3, 11);
                    multiShardingOrders.Add(new MultiShardingOrder()
                    {
                        Id = 232095129534607360,
                        Name = $"{now:yyyy/MM/dd HH:mm:ss}",
                        CreateTime = now
                    });
                }
                {
                    var now = new DateTime(2021, 10, 3, 7, 7, 7);
                    multiShardingOrders.Add(new MultiShardingOrder()
                    {
                        Id = 232398109278351360,
                        Name = $"{now:yyyy/MM/dd HH:mm:ss}",
                        CreateTime = now
                    });
                }
                {
                    var now = new DateTime(2021, 11, 6, 13, 13, 11);
                    multiShardingOrders.Add(new MultiShardingOrder()
                    {
                        Id = 244811420401807360,
                        Name = $"{now:yyyy/MM/dd HH:mm:ss}",
                        CreateTime = now
                    });
                }
                {
                    var now = new DateTime(2021, 11, 21, 19, 43, 0);
                    multiShardingOrders.Add(new MultiShardingOrder()
                    {
                        Id = 250345338962063360,
                        Name = $"{now:yyyy/MM/dd HH:mm:ss}",
                        CreateTime = now
                    });
                }
                {
                    var now = new DateTime(2021, 12, 5, 5, 5, 11);
                    multiShardingOrders.Add(new MultiShardingOrder()
                    {
                        Id = 255197859283087360,
                        Name = $"{now:yyyy/MM/dd HH:mm:ss}",
                        CreateTime = now
                    });
                }
                {
                    var now = new DateTime(2021, 12, 9, 19, 13, 11);
                    multiShardingOrders.Add(new MultiShardingOrder()
                    {
                        Id = 256860816933007360,
                        Name = $"{now:yyyy/MM/dd HH:mm:ss}",
                        CreateTime = now
                    });
                }
                {
                    var now = new DateTime(2021, 12, 19, 13, 13, 11);
                    multiShardingOrders.Add(new MultiShardingOrder()
                    {
                        Id = 260394098622607360,
                        Name = $"{now:yyyy/MM/dd HH:mm:ss}",
                        CreateTime = now
                    });
                }

                #endregion

                using (var tran = virtualDbContext.Database.BeginTransaction())
                {
                    await virtualDbContext.AddRangeAsync(userMods);
                    await virtualDbContext.AddRangeAsync(userModInts);
                    await virtualDbContext.AddRangeAsync(userSalaries);
                    await virtualDbContext.AddRangeAsync(orders);
                    await virtualDbContext.AddRangeAsync(logDays);
                    await virtualDbContext.AddRangeAsync(logDayLongs);
                    await virtualDbContext.AddRangeAsync(logWeeks);
                    await virtualDbContext.AddRangeAsync(logWeekLongs);
                    await virtualDbContext.AddRangeAsync(logYears);
                    await virtualDbContext.AddRangeAsync(logMonthLongs);
                    await virtualDbContext.AddRangeAsync(logYearkLongs);
                    await virtualDbContext.AddRangeAsync(multiShardingOrders);

                    await virtualDbContext.SaveChangesAsync();
                    tran.Commit();
                }
            }
        }
    }
}