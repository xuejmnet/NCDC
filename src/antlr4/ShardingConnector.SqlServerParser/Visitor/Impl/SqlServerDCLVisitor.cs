using System;
using System.Collections.Generic;
using ShardingConnector.AbstractParser;
using ShardingConnector.AbstractParser.Visitor.Commands;
using ShardingConnector.CommandParser.Command.DCL;
using ShardingConnector.CommandParser.Segment.Generic.Table;

namespace ShardingConnector.SqlServerParser.Visitor.Impl
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 20 April 2021 21:20:18
* @Email: 326308290@qq.com
*/
    public sealed class SqlServerDCLVisitor:SqlServerVisitor,IDCLVisitor
    {
        public override IASTNode VisitGrant(SqlServerCommandParser.GrantContext context)
        { 
            GrantCommand result = new GrantCommand();
            if (null != context.classPrivilegesClause()) {
                foreach (var simpleTableSegment in GetTableFromPrivilegeClause(context.classPrivilegesClause()))
                {
                    result.Tables.Add(simpleTableSegment);
                }
            }
            if (null != context.classTypePrivilegesClause()) {
                foreach (var simpleTableSegment in GetTableFromPrivilegeClause(context.classTypePrivilegesClause()))
                {
                    result.Tables.Add(simpleTableSegment);
                }
            }
            return result;
        }

        public override IASTNode VisitRevoke(SqlServerCommandParser.RevokeContext context)
        {
            RevokeCommand result = new RevokeCommand();
            if (null != context.classPrivilegesClause())
            {
                foreach (var simpleTableSegment in GetTableFromPrivilegeClause(context.classPrivilegesClause()))
                {
                    result.Tables.Add(simpleTableSegment);
                }
            }
            if (null != context.classTypePrivilegesClause())
            {
                foreach (var simpleTableSegment in GetTableFromPrivilegeClause(context.classTypePrivilegesClause()))
                {
                    result.Tables.Add(simpleTableSegment);
                }
            }
            return result;
        }

        private ICollection<SimpleTableSegment> GetTableFromPrivilegeClause( SqlServerCommandParser.ClassPrivilegesClauseContext ctx) {
            return null == ctx.onClassClause().tableName() ? new List<SimpleTableSegment>(0) :new List<SimpleTableSegment>(){(SimpleTableSegment)Visit(ctx.onClassClause().tableName())};
        }
    
        private ICollection<SimpleTableSegment> GetTableFromPrivilegeClause( SqlServerCommandParser.ClassTypePrivilegesClauseContext ctx) {
            return null == ctx.onClassTypeClause().tableName() ? new List<SimpleTableSegment>(0) : new List<SimpleTableSegment>(){(SimpleTableSegment)Visit(ctx.onClassTypeClause().tableName())};
        }

        public override IASTNode VisitCreateUser(SqlServerCommandParser.CreateUserContext context)
        {
            return new CreateUserCommand();
        }

        public override IASTNode VisitAlterUser(SqlServerCommandParser.AlterUserContext context)
        {
            return new AlterUserCommand();
        }

        public override IASTNode VisitDeny(SqlServerCommandParser.DenyContext context)
        {
            DenyUserCommand result = new DenyUserCommand();
            if (null != context.classPrivilegesClause())
            {
                foreach (var simpleTableSegment in GetTableFromPrivilegeClause(context.classPrivilegesClause()))
                {
                    result.Table = simpleTableSegment;
                }
            }
            if (null != context.classTypePrivilegesClause())
            {
                foreach (var simpleTableSegment in GetTableFromPrivilegeClause(context.classTypePrivilegesClause()))
                {
                    result.Table = simpleTableSegment;
                }
            }
            return result;
        }

        public override IASTNode VisitDropUser(SqlServerCommandParser.DropUserContext context)
        {
            return new DropUserCommand();
        }

        public override IASTNode VisitCreateRole(SqlServerCommandParser.CreateRoleContext context)
        {
            return new CreateRoleCommand();
        }

        public override IASTNode VisitAlterRole(SqlServerCommandParser.AlterRoleContext context)
        {
            return new AlterRoleCommand();
        }

        public override IASTNode VisitDropRole(SqlServerCommandParser.DropRoleContext context)
        {
            return new DropRoleCommand();
        }

        public override IASTNode VisitCreateLogin(SqlServerCommandParser.CreateLoginContext context)
        {
            return new CreateLoginCommand();
        }

        public override IASTNode VisitAlterLogin(SqlServerCommandParser.AlterLoginContext context)
        {
            return new AlterLoginCommand();
        }

        public override IASTNode VisitDropLogin(SqlServerCommandParser.DropLoginContext context)
        {
            return new DropLoginCommand();
        }
    }
}