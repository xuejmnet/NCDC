using System;
using System.Collections.Generic;
using System.Text;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Abstractions.Visitor.Commands;
using NCDC.CommandParser.Command.TCL;
using OpenConnector.Extensions;


namespace NCDC.SqlServerParser.Visitor.Impl
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/21 13:17:57
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class SqlServerTCLVisitor:SqlServerVisitor,ITCLVisitor
    {
        public override IASTNode VisitSetTransaction(SqlServerCommandParser.SetTransactionContext context)
        {
            return new SetTransactionCommand();
        }

        public override IASTNode VisitSetImplicitTransactions(SqlServerCommandParser.SetImplicitTransactionsContext context)
        {
            SetAutoCommitCommand result = new SetAutoCommitCommand();
            result.AutoCommit= "ON".EqualsIgnoreCase(context.implicitTransactionsValue().GetText());
            return result;
        }

        public override IASTNode VisitBeginTransaction(SqlServerCommandParser.BeginTransactionContext context)
        {
            return new BeginTransactionCommand();
        }

        public override IASTNode VisitCommit(SqlServerCommandParser.CommitContext context)
        {
            return new CommitCommand();
        }

        public override IASTNode VisitRollback(SqlServerCommandParser.RollbackContext context)
        {
            return new RollbackCommand();
        }

        public override IASTNode VisitSavepoint(SqlServerCommandParser.SavepointContext context)
        {
            return new SavepointCommand();
        }
    }
}
