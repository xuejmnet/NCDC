using System;
using System.Collections.Generic;
using System.Text;
using OpenConnector.Extensions;

namespace OpenConnector.ShardingRoute.Engine.Condition
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
        public  List<ShardingCondition> Conditions { get; }

        public ShardingConditions(List<ShardingCondition> conditions)
        {
            this.Conditions = conditions;
        }

        /**
         * Judge sharding conditions is always false or not.
         *
         * @return sharding conditions is always false or not
         */
        public bool IsAlwaysFalse()
        {
            if (Conditions.IsEmpty())
            {
                return false;
            }
            foreach (var condition in Conditions)
            {
                
                if (!(condition is AlwaysFalseShardingCondition)) {
                    return false;
                }
}
            return true;
        }
}
}
