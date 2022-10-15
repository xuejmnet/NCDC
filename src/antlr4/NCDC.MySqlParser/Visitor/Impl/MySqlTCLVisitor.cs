using System.Collections.ObjectModel;
using Antlr4.Runtime;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Abstractions.Visitor.Commands;
using NCDC.CommandParser.Common.Command.TCL;
using NCDC.CommandParser.Common.Constant;
using NCDC.CommandParser.Common.Segment.Generic;
using NCDC.CommandParser.Common.Segment.Generic.Table;
using NCDC.CommandParser.Common.Segment.TCL;
using NCDC.CommandParser.Common.Value.Identifier;
using NCDC.CommandParser.Dialect.Command.MySql.TCL;
using NCDC.Extensions;


namespace NCDC.MySqlParser.Visitor.Impl
{
    /// <summary>
    /// 
    /// </summary>
    /// Author: xjm
    /// Created: 2022/5/10 9:07:37
    /// Email: 326308290@qq.com
    public sealed class MySqlTCLVisitor : MySqlVisitor, ITCLVisitor
    {
        public override IASTNode VisitSetTransaction(MySqlCommandParser.SetTransactionContext ctx)
        {
            MySqlSetTransactionCommand result = new MySqlSetTransactionCommand();
            if (null != ctx.optionType())
            {
                OperationScopeEnum? scope = null;
                if (null != ctx.optionType().SESSION())
                {
                    scope = OperationScopeEnum.SESSION;
                }
                else if (null != ctx.optionType().GLOBAL())
                {
                    scope = OperationScopeEnum.GLOBAL;
                }

                result.Scope = scope;
            }

            if (null != ctx.transactionCharacteristics().isolationLevel())
            {
                TransactionIsolationLevelEnum? isolationLevel = null;
                if (null != ctx.transactionCharacteristics().isolationLevel().isolationTypes().SERIALIZABLE())
                {
                    isolationLevel = TransactionIsolationLevelEnum.SERIALIZABLE;
                }
                else if (null != ctx.transactionCharacteristics().isolationLevel().isolationTypes().COMMITTED())
                {
                    isolationLevel = TransactionIsolationLevelEnum.READ_COMMITTED;
                }
                else if (null != ctx.transactionCharacteristics().isolationLevel().isolationTypes().UNCOMMITTED())
                {
                    isolationLevel = TransactionIsolationLevelEnum.READ_UNCOMMITTED;
                }
                else if (null != ctx.transactionCharacteristics().isolationLevel().isolationTypes().REPEATABLE())
                {
                    isolationLevel = TransactionIsolationLevelEnum.REPEATABLE_READ;
                }

                result.IsolationLevel = isolationLevel;
            }

            if (null != ctx.transactionCharacteristics().transactionAccessMode())
            {
                TransactionAccessTypeEnum? accessType = null;
                if (null != ctx.transactionCharacteristics().transactionAccessMode().ONLY())
                {
                    accessType = TransactionAccessTypeEnum.READ_ONLY;
                }
                else if (null != ctx.transactionCharacteristics().transactionAccessMode().WRITE())
                {
                    accessType = TransactionAccessTypeEnum.READ_WRITE;
                }

                result.AccessMode = accessType;
            }

            return result;
        }


        public override IASTNode VisitSetAutoCommit(MySqlCommandParser.SetAutoCommitContext ctx)
        {
            var autoCommit = GenerateAutoCommitSegment(ctx.autoCommitValue).AutoCommit;
            return new MySqlSetAutoCommitCommand(autoCommit);
        }

        private AutoCommitSegment GenerateAutoCommitSegment(IToken ctx)
        {
            bool autoCommit = "1".Equals(ctx.Text) || "ON".Equals(ctx.Text);
            return new AutoCommitSegment(ctx.StartIndex, ctx.StopIndex, autoCommit);
        }


        public override IASTNode VisitBeginTransaction(MySqlCommandParser.BeginTransactionContext ctx)
        {
            return new MySqlBeginTransactionCommand();
        }


        public override IASTNode VisitCommit(MySqlCommandParser.CommitContext ctx)
        {
            return new MySqlCommitCommand();
        }


        public override IASTNode VisitRollback(MySqlCommandParser.RollbackContext ctx)
        {
            MySqlRollbackCommand result = new MySqlRollbackCommand();
            if (null != ctx.identifier())
            {
                result.SavepointName = ((IdentifierValue)Visit(ctx.identifier())).Value;
            }

            return result;
        }


        public override IASTNode VisitSavepoint(MySqlCommandParser.SavepointContext ctx)
        {
            var savepointName = ((IdentifierValue)Visit(ctx.identifier())).Value;
            return new MySqlSavepointCommand(savepointName);
        }


        public override IASTNode VisitXa(MySqlCommandParser.XaContext ctx)
        {
            var op = ctx.GetChild(1).GetText().ToUpper();
            MySqlXACommand result = new MySqlXACommand(op);
            if (null != ctx.xid())
            {
                result.XId = ctx.xid().GetText();
            }

            return result;
        }


        public override IASTNode VisitLock(MySqlCommandParser.LockContext ctx)
        {
            MySqlLockCommand result = new MySqlLockCommand();
            if (null != ctx.tableLock())
            {
                result.Tables.AddAll(GetLockTables(ctx.tableLock()));
            }

            return result;
        }

        private IEnumerable<SimpleTableSegment> GetLockTables(MySqlCommandParser.TableLockContext[] tableLockContexts)
        {
            foreach (var tableLockContext in tableLockContexts)
            {
                SimpleTableSegment simpleTableSegment = (SimpleTableSegment)Visit(tableLockContext.tableName());
                if (null != simpleTableSegment.GetAlias())
                {
                    simpleTableSegment.SetAlias((AliasSegment)Visit(tableLockContext.alias()));
                }

                yield return simpleTableSegment;
            }
        }


        public override IASTNode VisitUnlock(MySqlCommandParser.UnlockContext ctx)
        {
            return new MySqlUnlockCommand();
        }
    }
}