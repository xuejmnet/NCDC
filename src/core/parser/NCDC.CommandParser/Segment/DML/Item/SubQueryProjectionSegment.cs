using System;
using System.Collections.Generic;
using System.Text;
using NCDC.CommandParser.Segment.DML.Expr.SubQuery;
using NCDC.CommandParser.Segment.Generic;

namespace NCDC.CommandParser.Segment.DML.Item
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/21 12:39:25
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class SubQueryProjectionSegment:ISqlSegment,IProjectionSegment,IAliasAvailable
    {
        private readonly SubQuerySegment _subquery;
    
        
        private AliasSegment alias;

        public SubQueryProjectionSegment(SubQuerySegment subquery)
        {
            this._subquery = subquery;
        }

        public int GetStartIndex()
        {
            return _subquery.GetStartIndex();
        }

        public int GetStopIndex()
        {
            return _subquery.GetStopIndex();
        }

        public string GetAlias()
        {
            return alias?.GetIdentifier().GetValue();
        }

        public void SetAlias(AliasSegment alias)
        {
            this.alias = alias;
        }
    }
}
