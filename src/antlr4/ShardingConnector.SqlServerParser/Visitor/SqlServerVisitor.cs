using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.AbstractParser;
using ShardingConnector.CommandParser.Command.DML;
using ShardingConnector.CommandParser.Predicate;
using ShardingConnector.CommandParser.Segment.DDL.Index;
using ShardingConnector.CommandParser.Segment.DML.Column;
using ShardingConnector.CommandParser.Segment.DML.Expr;
using ShardingConnector.CommandParser.Segment.DML.Expr.Complex;
using ShardingConnector.CommandParser.Segment.DML.Expr.SubQuery;
using ShardingConnector.CommandParser.Segment.DML.Predicate.Value;
using ShardingConnector.CommandParser.Segment.Generic;
using ShardingConnector.CommandParser.Segment.Generic.Table;
using ShardingConnector.CommandParser.Segment.Predicate;
using ShardingConnector.CommandParser.Value.Collection;
using ShardingConnector.CommandParser.Value.Identifier;
using ShardingConnector.CommandParser.Value.Literal.Impl;
using ShardingConnector.CommandParser.Value.ParameterMaker;
using ShardingConnector.Exceptions;

namespace ShardingConnector.SqlServerParser.Visitor
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 15:03:53
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public abstract class SqlServerVisitor : SqlServerCommandBaseVisitor<IASTNode>
    {
        private int currentParameterIndex;
        public override IASTNode VisitParameterMarker(SqlServerCommandParser.ParameterMarkerContext context)
        {
            return new ParameterMarkerValue(currentParameterIndex++);
        }

        public override IASTNode VisitLiterals(SqlServerCommandParser.LiteralsContext context)
        {
            if (null != context.stringLiterals())
            {
                return Visit(context.stringLiterals());
            }
            if (null != context.numberLiterals())
            {
                return Visit(context.numberLiterals());
            }
            if (null != context.hexadecimalLiterals())
            {
                return Visit(context.hexadecimalLiterals());
            }
            if (null != context.bitValueLiterals())
            {
                return Visit(context.bitValueLiterals());
            }
            if (null != context.booleanLiterals())
            {
                return Visit(context.booleanLiterals());
            }
            if (null != context.nullValueLiterals())
            {
                return Visit(context.nullValueLiterals());
            }
            throw new ShardingException("Literals must have string, number, dateTime, hex, bit, boolean or null.");
        }

        public override IASTNode VisitStringLiterals(SqlServerCommandParser.StringLiteralsContext context)
        {
            return new StringLiteralValue(context.GetText());
        }

        public override IASTNode VisitNumberLiterals(SqlServerCommandParser.NumberLiteralsContext context)
        {
            return new NumberLiteralValue(context.GetText());
        }

        public override IASTNode VisitHexadecimalLiterals(SqlServerCommandParser.HexadecimalLiteralsContext context)
        {
            return new OtherLiteralValue(context.GetText());
        }

        public override IASTNode VisitBitValueLiterals(SqlServerCommandParser.BitValueLiteralsContext context)
        {
            return new OtherLiteralValue(context.GetText());
        }

        public override IASTNode VisitBooleanLiterals(SqlServerCommandParser.BooleanLiteralsContext context)
        {
            return new BooleanLiteralValue(context.GetText());
        }

        public override IASTNode VisitNullValueLiterals(SqlServerCommandParser.NullValueLiteralsContext context)
        {
            return new OtherLiteralValue(context.GetText());
        }

        public override IASTNode VisitIdentifier(SqlServerCommandParser.IdentifierContext context)
        {
            var unreservedWord = context.unreservedWord();
            return null != unreservedWord ? Visit(unreservedWord) : new IdentifierValue(context.GetText());
        }

        public override IASTNode VisitUnreservedWord(SqlServerCommandParser.UnreservedWordContext context)
        {
            return new IdentifierValue(context.GetText());
        }

        public override IASTNode VisitSchemaName(SqlServerCommandParser.SchemaNameContext context)
        {
            return Visit(context.identifier());
        }

        public override IASTNode VisitTableName(SqlServerCommandParser.TableNameContext context)
        {
            SimpleTableSegment result = new SimpleTableSegment(new TableNameSegment(context.Start.StartIndex, context.Stop.StopIndex, (IdentifierValue)Visit(context.name())));
            SqlServerCommandParser.OwnerContext owner = context.owner();
            if (null != owner)
            {
                result.SetOwner(new OwnerSegment(owner.Start.StartIndex, owner.Stop.StopIndex, (IdentifierValue)Visit(owner.identifier())));
            }
            return result;
        }

        public override IASTNode VisitColumnName(SqlServerCommandParser.ColumnNameContext context)
        {
            ColumnSegment result = new ColumnSegment(context.Start.StartIndex, context.Stop.StopIndex, (IdentifierValue)Visit(context.name()));
            SqlServerCommandParser.OwnerContext owner = context.owner();
            if (null != owner)
            {
                result.SetOwner(new OwnerSegment(owner.Start.StartIndex, owner.Stop.StopIndex, (IdentifierValue)Visit(owner.identifier())));
            }
            return result;
        }

        public override IASTNode VisitIndexName(SqlServerCommandParser.IndexNameContext context)
        {
            return new IndexSegment(context.Start.StartIndex, context.Stop.StopIndex, (IdentifierValue)Visit(context.identifier()));
        }

        public override IASTNode VisitTableNames(SqlServerCommandParser.TableNamesContext context)
        {
            CollectionValue<SimpleTableSegment> result = new CollectionValue<SimpleTableSegment>();
            foreach (var tableName in context.tableName())
            {
                result.GetValue().Add((SimpleTableSegment)Visit(tableName));
            }
            return result;
        }

        public override IASTNode VisitColumnNames(SqlServerCommandParser.ColumnNamesContext context)
        {
            CollectionValue<ColumnSegment> result = new CollectionValue<ColumnSegment>();
            foreach (var columnName in context.columnNameWithSort())
            {
                result.GetValue().Add((ColumnSegment)Visit(columnName));
            }
            return result;
        }

        public override IASTNode VisitExpr(SqlServerCommandParser.ExprContext context)
        {
            if (null != context.booleanPrimary())
            {
                return Visit(context.booleanPrimary());
            }
            if (null != context.logicalOperator())
            {
                return new PredicateBuilder(Visit(context.expr(0)), Visit(context.expr(1)), context.logicalOperator().GetText()).MergePredicate();
            }
            // TODO deal with XOR
            return Visit(context.expr()[0]);
        }

        public override IASTNode VisitBooleanPrimary(SqlServerCommandParser.BooleanPrimaryContext context)
        {
            if (null != context.comparisonOperator() || null != context.SAFE_EQ_())
            {
                return CreateCompareSegment(context);
            }
            if (null != context.predicate())
            {
                return Visit(context.predicate());
            }
            if (null != context.subquery())
            {
                return new SubQuerySegment(context.Start.StartIndex, context.Stop.StopIndex, (SelectCommand)Visit(context.subquery()));
            }
            //TODO deal with IS NOT? (TRUE | FALSE | UNKNOWN | NULL)
            return new CommonExpressionSegment(context.Start.StartIndex, context.Stop.StopIndex, context.GetText());
        }
        private IASTNode CreateCompareSegment(SqlServerCommandParser.BooleanPrimaryContext context)
        {
            IASTNode leftValue = Visit(context.booleanPrimary());
            if (leftValue is ColumnSegment columnSegment)
            {

                var rightValue = (IPredicateRightValue)CreatePredicateRightValue(context);
                return new PredicateSegment(context.Start.StartIndex, context.Stop.StopIndex, columnSegment, rightValue);
            }
            else
            {
                return leftValue;
            }
        }
        private IASTNode CreatePredicateRightValue(SqlServerCommandParser.BooleanPrimaryContext context)
        {
            if (null != context.subquery())
            {
                return new SubQuerySegment(context.Start.StartIndex, context.Stop.StopIndex, (SelectCommand)Visit(context.subquery()));
            }
            IASTNode rightValue = Visit(context.predicate());
            return CreatePredicateRightValue(context, rightValue);
        }
        private IASTNode CreatePredicateRightValue(SqlServerCommandParser.BooleanPrimaryContext context, IASTNode rightValue)
        {
            if (rightValue is ColumnSegment columnSegment)
            {
                return columnSegment;
            }
            return rightValue is SubQuerySegment subQuerySegment ? new PredicateCompareRightValue(context.comparisonOperator().GetText(), new SubQueryExpressionSegment((SubQuerySegment)rightValue))
                : new PredicateCompareRightValue(context.comparisonOperator().GetText(), (IExpressionSegment)rightValue);
        }

        public override IASTNode VisitPredicate(SqlServerCommandParser.PredicateContext context)
        {
            if (null != context.IN() && null == context.NOT())
            {
                return CreateInSegment(context);
            }
            if (null != context.BETWEEN() && null == context.NOT())
            {
                return CreateBetweenSegment(context);
            }
            if (1 == context.children.Count)
            {
                return Visit(context.bitExpr(0));
            }
            return VisitRemainPredicate(context);
        }
        private PredicateSegment CreateInSegment(SqlServerCommandParser.PredicateContext context)
        {
            ColumnSegment column = (ColumnSegment)Visit(context.bitExpr(0));
            PredicateBracketValue predicateBracketValue = CreateBracketValue(context);
            return new PredicateSegment(context.Start.StartIndex, context.Stop.StopIndex, column, new PredicateInRightValue(predicateBracketValue, GetExpressionSegments(context)));
        }
        private PredicateBracketValue CreateBracketValue(SqlServerCommandParser.PredicateContext context)
        {
            PredicateLeftBracketValue predicateLeftBracketValue = null != context.subquery()
                ? new PredicateLeftBracketValue(context.subquery().LP_().Symbol.StartIndex, context.subquery().LP_().Symbol.StopIndex)
                : new PredicateLeftBracketValue(context.LP_().Symbol.StartIndex, context.LP_().Symbol.StopIndex);
            PredicateRightBracketValue predicateRightBracketValue = null != context.subquery()
                ? new PredicateRightBracketValue(context.subquery().RP_().Symbol.StartIndex, context.subquery().RP_().Symbol.StopIndex)
                : new PredicateRightBracketValue(context.RP_().Symbol.StartIndex, context.RP_().Symbol.StopIndex);
            return new PredicateBracketValue(predicateLeftBracketValue, predicateRightBracketValue);
        }
        private ICollection<IExpressionSegment> GetExpressionSegments(SqlServerCommandParser.PredicateContext context)
        {
            ICollection<IExpressionSegment> result = new LinkedList<IExpressionSegment>();
            if (null != context.subquery())
            {
                SqlServerCommandParser.SubqueryContext subquery = context.subquery();
                result.Add(new SubQueryExpressionSegment(new SubQuerySegment(subquery.Start.StartIndex, subquery.Stop.StopIndex, (SelectCommand)Visit(context.subquery()))));
                return result;
            }
            foreach (var expr in context.expr())
            {
                result.Add((IExpressionSegment)Visit(expr));
            }
            return result;
        }
        private PredicateSegment CreateBetweenSegment(SqlServerCommandParser.PredicateContext context)
        {
            ColumnSegment column = (ColumnSegment)Visit(context.bitExpr(0));
            IExpressionSegment between = (IExpressionSegment)Visit(context.bitExpr(1));
            IExpressionSegment and = (IExpressionSegment)Visit(context.predicate());
            return new PredicateSegment(context.Start.StartIndex, context.Stop.StopIndex, column, new PredicateBetweenRightValue(between, and));
        }
        private IASTNode VisitRemainPredicate(SqlServerCommandParser.PredicateContext context)
        {
            foreach (var bitExpr in context.bitExpr())
            {
                Visit(bitExpr);
            }
            foreach (var expr in context.expr())
            {
                Visit(expr);
            }
            foreach (var simpleExpr in context.simpleExpr())
            {
                Visit(simpleExpr);
            }
            if (null != context.predicate())
            {
                Visit(context.predicate());
            }
            return new CommonExpressionSegment(context.Start.StartIndex, context.Stop.StopIndex, context.GetText());
        }
    }
}
