using System;
using System.Collections.Generic;
using NCDC.ShardingTest.Seed.Entities;
using NCDC.ShardingTest.Seed.Shardings.PaginationConfigs;
using ShardingCore.Core.EntityMetadatas;
using ShardingCore.Sharding.PaginationConfigurations;
using ShardingCore.VirtualRoutes.Days;

namespace NCDC.ShardingTest.Seed.Shardings
{
    public class LogDayVirtualTableRoute:AbstractSimpleShardingDayKeyDateTimeVirtualTableRoute<LogDay>
    {
        //public override bool? EnableRouteParseCompileCache => true;
        protected override bool EnableHintRoute => true;

        public override DateTime GetBeginTime()
        {
            return new DateTime(2021, 1, 1);
        }
        

        public override void Configure(EntityMetadataTableBuilder<LogDay> builder)
        {
            builder.ShardingProperty(o => o.LogTime);
            builder.TableSeparator(string.Empty);
        }

        public override IPaginationConfiguration<LogDay> CreatePaginationConfiguration()
        {
            return new LogDayPaginationConfiguration();
        }

        public override bool AutoCreateTableByTime()
        {
            return true;
        }

        protected override List<string> CalcTailsOnStart()
        {
            var beginTime = GetBeginTime().Date;

            var tails = new List<string>();
            //提前创建表
            var nowTimeStamp = new DateTime(2021,11,20).Date;
            if (beginTime > nowTimeStamp)
                throw new ArgumentException("begin time error");
            var currentTimeStamp = beginTime;
            while (currentTimeStamp <= nowTimeStamp)
            {
                var tail = ShardingKeyToTail(currentTimeStamp);
                tails.Add(tail);
                currentTimeStamp = currentTimeStamp.AddDays(1);
            }
            return tails;
        }
    }
}
