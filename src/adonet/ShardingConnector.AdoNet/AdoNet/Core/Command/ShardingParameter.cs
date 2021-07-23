using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace ShardingConnector.AdoNet.AdoNet.Core.Command
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/7/23 15:56:13
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public class ShardingParameter: DbParameter
    {
        public override void ResetDbType()
        {
            throw new NotImplementedException();
        }

        public override DbType DbType { get; set; }
        public override ParameterDirection Direction { get; set; }
        public override bool IsNullable { get; set; }
        public override string ParameterName { get; set; }
        public override string SourceColumn { get; set; }
        public override object Value { get; set; }
        public override bool SourceColumnNullMapping { get; set; }
        public override int Size { get; set; }
    }
}
