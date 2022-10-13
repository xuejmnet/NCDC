using System;
using NCDC.CommandParser.Common.Constant;

namespace NCDC.CommandParser.Common.Command.TCL
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 20 April 2021 22:18:32
* @Email: 326308290@qq.com
*/
    public abstract class SetTransactionCommand: AbstractSqlCommand, ITCLCommand
    {
        public TransactionIsolationLevelEnum? IsolationLevel { get; set; }
        public OperationScopeEnum? Scope { get; set; }
        public TransactionAccessTypeEnum? AccessMode { get; set; }
    }
}