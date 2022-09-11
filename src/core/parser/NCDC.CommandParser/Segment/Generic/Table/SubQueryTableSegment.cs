using System;
using System.Collections.Generic;
using System.Text;
using NCDC.CommandParser.Segment.DML.Expr.SubQuery;

namespace NCDC.CommandParser.Segment.Generic.Table
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/21 13:04:48
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class SubQueryTableSegment:ITableSegment,IAliasAvailable
    {
        private readonly SubQuerySegment _subquery;
    
        private AliasSegment alias;

        public SubQueryTableSegment(SubQuerySegment subquery)
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
