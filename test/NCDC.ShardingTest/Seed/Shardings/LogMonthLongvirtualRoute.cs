﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NCDC.ShardingTest.Seed.Entities;
using ShardingCore.Core.EntityMetadatas;
using ShardingCore.VirtualRoutes.Months;

namespace NCDC.ShardingTest.Seed.Shardings
{
    public class LogMonthLongvirtualRoute:AbstractSimpleShardingMonthKeyLongVirtualTableRoute<LogMonthLong>
    {
        //public override bool? EnableRouteParseCompileCache => true;
        protected override bool EnableHintRoute => true;

        public override bool AutoCreateTableByTime()
        {
            return true;
        }

        public override DateTime GetBeginTime()
        {
            return new DateTime(2021, 1, 1);
        }

        public override void Configure(EntityMetadataTableBuilder<LogMonthLong> builder)
        {
            builder.ShardingProperty(o => o.LogTime);
        }
    }
}
