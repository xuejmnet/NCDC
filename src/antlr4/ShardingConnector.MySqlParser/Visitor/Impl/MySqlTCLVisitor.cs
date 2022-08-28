using ShardingConnector.CommandParser.Command.TCL;
using ShardingConnector.CommandParser.Segment.TCL;
using ShardingConnector.Parsers;
using ShardingConnector.Parsers.Visitor.Commands;

namespace ShardingConnector.MySqlParser.Visitor.Impl
{
    /// <summary>
    /// 
    /// </summary>
    /// Author: xjm
    /// Created: 2022/5/10 9:07:37
    /// Email: 326308290@qq.com
    public sealed class MySqlTCLVisitor : MySqlVisitor, ITCLVisitor
    {
        public override IASTNode VisitSetTransaction( MySqlCommandParser.SetTransactionContext ctx) {
            return new SetTransactionCommand();
        }
    
        
        public override IASTNode VisitSetAutoCommit( MySqlCommandParser.SetAutoCommitContext ctx) {
            SetAutoCommitCommand result = new SetAutoCommitCommand();
            result.AutoCommit = ((AutoCommitSegment)Visit(ctx.autoCommitValue())).IsAutoCommit();
            return result;
        }
    
        
        public override IASTNode VisitAutoCommitValue( MySqlCommandParser.AutoCommitValueContext ctx) {
            var autoCommit = "1".Equals(ctx.GetText()) || "ON".Equals(ctx.GetText());
            return new AutoCommitSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, autoCommit);
        }
    
        
        public override IASTNode VisitBeginTransaction( MySqlCommandParser.BeginTransactionContext ctx) {
            return new BeginTransactionCommand();
        }
    
        
        public override IASTNode VisitCommit( MySqlCommandParser.CommitContext ctx) {
            return new CommitCommand();
        }
    
        
        public override IASTNode VisitRollback( MySqlCommandParser.RollbackContext ctx) {
            return new RollbackCommand();
        }
    
        
        public override IASTNode VisitSavepoint( MySqlCommandParser.SavepointContext ctx) {
            return new SavepointCommand();
        }
    }
}
