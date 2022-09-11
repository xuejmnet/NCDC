using System;
using NCDC.CommandParser.Constant;

namespace OpenConnector.ShardingMerge.DQL.OrderBy
{
/*
* @Author: xjm
* @Description:
* @Date: Friday, 07 May 2021 21:27:39
* @Email: 326308290@qq.com
*/
    public class CompareUtil
    {
        private CompareUtil()
        {
            
        }
        public static int CompareTo(IComparable thisValue, IComparable otherValue, OrderDirectionEnum orderDirection, OrderDirectionEnum nullOrderDirection, bool caseSensitive) {
            if (null == thisValue && null == otherValue) {
                return 0;
            }
            if (null == thisValue) {
                return orderDirection == nullOrderDirection ? -1 : 1;
            }
            if (null == otherValue) {
                return orderDirection == nullOrderDirection ? 1 : -1;
            }
            if (!caseSensitive && thisValue is string && otherValue is string) {
                return CompareToCaseInsensitiveString((string)thisValue, (string)otherValue, orderDirection);
            }
            return OrderDirectionEnum.ASC == orderDirection ? thisValue.CompareTo(otherValue) : -thisValue.CompareTo(otherValue);
        }
    
        private static int CompareToCaseInsensitiveString(string thisValue, string otherValue, OrderDirectionEnum orderDirection) {
            int result = String.Compare(thisValue.ToUpper(), otherValue.ToUpper(), StringComparison.Ordinal);
            return OrderDirectionEnum.ASC == orderDirection ? result : -result;
        }
    }
}