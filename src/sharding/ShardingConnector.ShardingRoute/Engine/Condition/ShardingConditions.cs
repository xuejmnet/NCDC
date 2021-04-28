using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Extensions;

namespace ShardingConnector.ShardingRoute.Engine.Condition
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/28 11:45:39
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ShardingConditions
    {
        private readonly List<ShardingCondition> conditions;

        public ShardingConditions(List<ShardingCondition> conditions)
        {
            this.conditions = conditions;
        }

        /**
         * Judge sharding conditions is always false or not.
         *
         * @return sharding conditions is always false or not
         */
        public bool isAlwaysFalse()
        {
            if (conditions.IsEmpty())
            {
                return false;
            }
            foreach (var condition in conditions)
            {
                
                if (!(condition is AlwaysFalseShardingCondition)) {
                    return false;
                }
}
            return true;
        }
}
}
