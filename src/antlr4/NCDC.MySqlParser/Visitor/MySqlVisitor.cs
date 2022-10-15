using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Common.Constant;
using NCDC.CommandParser.Common.Segment.DDL.Constraint;
using NCDC.CommandParser.Common.Segment.DDL.Index;
using NCDC.CommandParser.Common.Segment.DML.Assignment;
using NCDC.CommandParser.Common.Segment.DML.Column;
using NCDC.CommandParser.Common.Segment.DML.Combine;
using NCDC.CommandParser.Common.Segment.DML.Expr;
using NCDC.CommandParser.Common.Segment.DML.Expr.Complex;
using NCDC.CommandParser.Common.Segment.DML.Expr.Simple;
using NCDC.CommandParser.Common.Segment.DML.Expr.SubQuery;
using NCDC.CommandParser.Common.Segment.DML.Item;
using NCDC.CommandParser.Common.Segment.DML.Order;
using NCDC.CommandParser.Common.Segment.DML.Order.Item;
using NCDC.CommandParser.Common.Segment.DML.Pagination;
using NCDC.CommandParser.Common.Segment.DML.Pagination.limit;
using NCDC.CommandParser.Common.Segment.DML.Predicate;
using NCDC.CommandParser.Common.Segment.DML.Predicate.Value;
using NCDC.CommandParser.Common.Segment.Generic;
using NCDC.CommandParser.Common.Segment.Generic.Table;
using NCDC.CommandParser.Common.Util;
using NCDC.CommandParser.Common.Value.Collection;
using NCDC.CommandParser.Common.Value.Identifier;
using NCDC.CommandParser.Common.Value.KeyWord;
using NCDC.CommandParser.Common.Value.Literal.Impl;
using NCDC.CommandParser.Common.Value.ParameterMaker;
using NCDC.CommandParser.Dialect.Command.MySql.DML;
using NCDC.Exceptions;
using NCDC.Extensions;


namespace NCDC.MySqlParser.Visitor
{
    /// <summary>
    /// 
    /// </summary>
    /// Author: xjm
    /// Created: 2022/5/10 8:09:26
    /// Email: 326308290@qq.com
    public class MySqlVisitor : MySqlCommandBaseVisitor<IASTNode>
    {
        private int _currentParameterIndex;

        protected ICollection<IParameterMarkerSegment> ParameterMarkerSegments =
            new LinkedList<IParameterMarkerSegment>();

        public int GetCurrentParameterIndex()
        {
            return _currentParameterIndex;
        }

        public override IASTNode VisitParameterMarker(MySqlCommandParser.ParameterMarkerContext ctx)
        {
            return new ParameterMarkerValue(_currentParameterIndex++, ParameterMarkerTypeEnum.AT,ctx.GetText());
        }


        public override IASTNode VisitLiterals(MySqlCommandParser.LiteralsContext ctx)
        {
            if (null != ctx.stringLiterals())
            {
                return Visit(ctx.stringLiterals());
            }

            if (null != ctx.numberLiterals())
            {
                return Visit(ctx.numberLiterals());
            }

            if (null != ctx.temporalLiterals())
            {
                return Visit(ctx.temporalLiterals());
            }

            if (null != ctx.hexadecimalLiterals())
            {
                return Visit(ctx.hexadecimalLiterals());
            }

            if (null != ctx.bitValueLiterals())
            {
                return Visit(ctx.bitValueLiterals());
            }

            if (null != ctx.booleanLiterals())
            {
                return Visit(ctx.booleanLiterals());
            }

            if (null != ctx.nullValueLiterals())
            {
                return Visit(ctx.nullValueLiterals());
            }

            throw new InvalidOperationException(
                "Literals must have string, number, dateTime, hex, bit, boolean or null.");
        }


        public override IASTNode VisitStringLiterals(MySqlCommandParser.StringLiteralsContext ctx)
        {
            return new StringLiteralValue(ctx.GetText());
        }


        public override IASTNode VisitString_(MySqlCommandParser.String_Context ctx)
        {
            return new StringLiteralValue(ctx.GetText());
        }


        public override IASTNode VisitNumberLiterals(MySqlCommandParser.NumberLiteralsContext ctx)
        {
            return new NumberLiteralValue(ctx.GetText());
        }


        public override IASTNode VisitTemporalLiterals(MySqlCommandParser.TemporalLiteralsContext ctx)
        {
            // TODO deal with TemporalLiterals
            return new OtherLiteralValue(ctx.GetText());
        }


        public override IASTNode VisitHexadecimalLiterals(MySqlCommandParser.HexadecimalLiteralsContext ctx)
        {
            // TODO deal with hexadecimalLiterals
            return new OtherLiteralValue(ctx.GetText());
        }


        public override IASTNode VisitBitValueLiterals(MySqlCommandParser.BitValueLiteralsContext ctx)
        {
            // TODO deal with bitValueLiterals
            return new OtherLiteralValue(ctx.GetText());
        }


        public override IASTNode VisitBooleanLiterals(MySqlCommandParser.BooleanLiteralsContext ctx)
        {
            return new BooleanLiteralValue(ctx.GetText());
        }


        public override IASTNode VisitNullValueLiterals(MySqlCommandParser.NullValueLiteralsContext ctx)
        {
            // TODO deal with nullValueLiterals
            return new OtherLiteralValue(ctx.GetText());
        }


        public override IASTNode VisitIdentifier(MySqlCommandParser.IdentifierContext ctx)
        {
            return new IdentifierValue(ctx.GetText());
        }


        public override IASTNode VisitSchemaName(MySqlCommandParser.SchemaNameContext ctx)
        {
            return new DatabaseSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex,
                (IdentifierValue)Visit(ctx.identifier()));
        }


        public override IASTNode VisitTableName(MySqlCommandParser.TableNameContext ctx)
        {
            SimpleTableSegment result = new SimpleTableSegment(new TableNameSegment(ctx.name().Start.StartIndex,
                ctx.name().Stop.StopIndex, new IdentifierValue(ctx.name().identifier().GetText())));
            MySqlCommandParser.OwnerContext owner = ctx.owner();
            if (null != owner)
            {
                result.Owner = (OwnerSegment)Visit(owner);
            }

            return result;
        }


        public override IASTNode VisitViewName(MySqlCommandParser.ViewNameContext ctx)
        {
            SimpleTableSegment result = new SimpleTableSegment(new TableNameSegment(ctx.identifier().Start.StartIndex,
                ctx.identifier().Stop.StopIndex, new IdentifierValue(ctx.identifier().GetText())));
            MySqlCommandParser.OwnerContext owner = ctx.owner();
            if (null != owner)
            {
                result.Owner = (OwnerSegment)Visit(owner);
            }

            return result;
        }


        public override IASTNode VisitOwner(MySqlCommandParser.OwnerContext ctx)
        {
            return new OwnerSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, (IdentifierValue)Visit(ctx.identifier()));
        }


        public override IASTNode VisitFunctionName(MySqlCommandParser.FunctionNameContext ctx)
        {
            FunctionSegment result = new FunctionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex,
                ctx.identifier().IDENTIFIER_().GetText(), ctx.GetText());
            if (null != ctx.owner())
            {
                result.Owner = (OwnerSegment)Visit(ctx.owner());
            }

            return result;
        }


        public override IASTNode VisitColumnName(MySqlCommandParser.ColumnNameContext ctx)
        {
            return new ColumnSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex,
                (IdentifierValue)Visit(ctx.identifier()));
        }


        public override IASTNode VisitIndexName(MySqlCommandParser.IndexNameContext ctx)
        {
            IndexNameSegment indexName = new IndexNameSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex,
                (IdentifierValue)Visit(ctx.identifier()));
            return new IndexSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, indexName);
        }


        public override IASTNode VisitTableList(MySqlCommandParser.TableListContext ctx)
        {
            CollectionValue<SimpleTableSegment> result = new CollectionValue<SimpleTableSegment>();
            foreach (var tableNameContext in ctx.tableName())
            {
                result.Value.Add((SimpleTableSegment)Visit(tableNameContext));
            }

            return result;
        }


        public override IASTNode VisitViewNames(MySqlCommandParser.ViewNamesContext ctx)
        {
            CollectionValue<SimpleTableSegment> result = new CollectionValue<SimpleTableSegment>();
            foreach (var viewNameContext in ctx.viewName())
            {
                result.Value.Add((SimpleTableSegment)Visit(viewNameContext));
            }

            return result;
        }


        public override IASTNode VisitColumnNames(MySqlCommandParser.ColumnNamesContext ctx)
        {
            CollectionValue<ColumnSegment> result = new CollectionValue<ColumnSegment>();
            foreach (var columnNameContext in ctx.columnName())
            {
                result.Value.Add((ColumnSegment)Visit(columnNameContext));
            }

            return result;
        }


        public override IASTNode VisitExpr(MySqlCommandParser.ExprContext ctx)
        {
            if (null != ctx.booleanPrimary())
            {
                return Visit(ctx.booleanPrimary());
            }

            if (null != ctx.XOR())
            {
                return CreateBinaryOperationExpression(ctx, "XOR");
            }

            if (null != ctx.andOperator())
            {
                return CreateBinaryOperationExpression(ctx, ctx.andOperator().GetText());
            }

            if (null != ctx.orOperator())
            {
                return CreateBinaryOperationExpression(ctx, ctx.orOperator().GetText());
            }

            return new NotExpression(ctx.Start.StartIndex, ctx.Stop.StopIndex, (IExpressionSegment)Visit(ctx.expr(0)));
        }

        private BinaryOperationExpression CreateBinaryOperationExpression(MySqlCommandParser.ExprContext ctx,
            String @operator)
        {
            IExpressionSegment left = (IExpressionSegment)Visit(ctx.expr(0));
            IExpressionSegment right = (IExpressionSegment)Visit(ctx.expr(1));
            String text = ctx.Start.InputStream.GetText(new Interval(ctx.Start.StartIndex, ctx.Stop.StopIndex));
            return new BinaryOperationExpression(ctx.Start.StartIndex, ctx.Stop.StopIndex, left, right,
  @operator, text);
        }


        public override IASTNode VisitBooleanPrimary(MySqlCommandParser.BooleanPrimaryContext ctx)
        {
            if (null != ctx.IS())
            {
                String rightText = "";
                if (null != ctx.NOT())
                {
                    rightText =rightText+ctx.Start.InputStream.GetText(new Interval(
                        ctx.NOT().Symbol.StartIndex,
                        ctx.NOT().Symbol.StopIndex))+" ";
                }

                IToken operatorToken = null;
                if (null != ctx.NULL())
                {
                    operatorToken = ctx.NULL().Symbol;
                }

                if (null != ctx.TRUE())
                {
                    operatorToken = ctx.TRUE().Symbol;
                }

                if (null != ctx.FALSE())
                {
                    operatorToken = ctx.FALSE().Symbol;
                }

                int startIndex = null == operatorToken
                    ? ctx.IS().Symbol.StopIndex + 1
                    : operatorToken.StartIndex;
                rightText = rightText+ctx.Start.InputStream
                    .GetText(new Interval(startIndex, ctx.Stop.StopIndex));
                IExpressionSegment right = new LiteralExpressionSegment(ctx.IS().Symbol.StopIndex + 1,
                    ctx.Stop.StopIndex, rightText);
                String text = ctx.Start.InputStream
                    .GetText(new Interval(ctx.Start.StartIndex, ctx.Stop.StopIndex));
                IExpressionSegment left = (IExpressionSegment)Visit(ctx.booleanPrimary());
                String @operator = "IS";
                return new BinaryOperationExpression(ctx.Start.StartIndex, ctx.Stop.StopIndex, left, right,
  @operator, text);
            }

            if (null != ctx.comparisonOperator() || null != ctx.SAFE_EQ_())
            {
                return CreateCompareSegment(ctx);
            }

            if (null != ctx.assignmentOperator())
            {
                return CreateAssignmentSegment(ctx);
            }

            return Visit(ctx.predicate());
        }

        private IASTNode CreateAssignmentSegment(MySqlCommandParser.BooleanPrimaryContext ctx)
        {
            IExpressionSegment left = (IExpressionSegment)Visit(ctx.booleanPrimary());
            IExpressionSegment right = (IExpressionSegment)Visit(ctx.predicate());
            String @operator = ctx.assignmentOperator().GetText();
            String text = ctx.Start.InputStream.GetText(new Interval(ctx.Start.StartIndex, ctx.Stop.StopIndex));
            return new BinaryOperationExpression(ctx.Start.StartIndex, ctx.Stop.StopIndex, left, right,
  @operator, text);
        }

        private IASTNode CreateCompareSegment(MySqlCommandParser.BooleanPrimaryContext ctx)
        {
            IExpressionSegment left = (IExpressionSegment)Visit(ctx.booleanPrimary());
            IExpressionSegment right;
            if (null != ctx.predicate())
            {
                right = (IExpressionSegment)Visit(ctx.predicate());
            }
            else
            {
                right = new SubQueryExpressionSegment(new SubQuerySegment(ctx.subquery().Start.StartIndex,
                    ctx.subquery().Stop.StopIndex, (MySqlSelectCommand)Visit(ctx.subquery())));
            }

            String @operator = null != ctx.SAFE_EQ_() ? ctx.SAFE_EQ_().GetText() : ctx.comparisonOperator().GetText();
            String text = ctx.Start.InputStream.GetText(new Interval(ctx.Start.StartIndex, ctx.Stop.StopIndex));
            return new BinaryOperationExpression(ctx.Start.StartIndex, ctx.Stop.StopIndex, left, right,
  @operator, text);
        }


        public override IASTNode VisitPredicate(MySqlCommandParser.PredicateContext ctx)
        {
            if (null != ctx.IN())
            {
                return createInSegment(ctx);
            }

            if (null != ctx.BETWEEN())
            {
                return createBetweenSegment(ctx);
            }

            if (null != ctx.LIKE())
            {
                return CreateBinaryOperationExpressionFromLike(ctx);
            }

            if (null != ctx.REGEXP())
            {
                return createBinaryOperationExpressionFromRegexp(ctx);
            }

            return Visit(ctx.bitExpr(0));
        }

        private InExpression createInSegment(MySqlCommandParser.PredicateContext ctx)
        {
            bool not = null != ctx.NOT();
            IExpressionSegment left = (IExpressionSegment)Visit(ctx.bitExpr(0));
            IExpressionSegment right;
            if (null != ctx.subquery())
            {
                right = new SubQueryExpressionSegment(new SubQuerySegment(ctx.subquery().Start.StartIndex,
                    ctx.subquery().Stop.StopIndex, (MySqlSelectCommand)Visit(ctx.subquery())));
            }
            else
            {
                right = new ListExpression(ctx.LP_().Symbol.StartIndex, ctx.RP_().Symbol.StopIndex);
                foreach (var exprContext in ctx.expr())
                {
                    ((ListExpression)right).Items.Add((IExpressionSegment)Visit(exprContext));
                }
              
            }

            return new InExpression(ctx.Start.StartIndex, ctx.Stop.StopIndex, left, right, not);
        }

        private BinaryOperationExpression CreateBinaryOperationExpressionFromLike(
            MySqlCommandParser.PredicateContext ctx)
        {
            IExpressionSegment left = (IExpressionSegment)Visit(ctx.bitExpr(0));
            String @operator;
            IExpressionSegment right;
            if (null != ctx.SOUNDS())
            {
                right = (IExpressionSegment)Visit(ctx.bitExpr(1));
                    @operator = "SOUNDS LIKE";
            }
            else
            {
                ListExpression listExpression = new ListExpression(ctx.simpleExpr(0).Start.StartIndex,
                    ctx.simpleExpr()[ctx.simpleExpr().Length - 1].Stop.StopIndex);
                foreach (var simpleExprContext in ctx.simpleExpr())
                {
                    listExpression.Items.Add((IExpressionSegment)Visit(simpleExprContext));
                }
                right = listExpression;
                    @operator = null != ctx.NOT() ? "NOT LIKE" : "LIKE";
            }

            String text = ctx.Start.InputStream.GetText(new Interval(ctx.Start.StartIndex, ctx.Stop.StopIndex));
            return new BinaryOperationExpression(ctx.Start.StartIndex, ctx.Stop.StopIndex, left, right,
  @operator, text);
        }

        private BinaryOperationExpression createBinaryOperationExpressionFromRegexp(
            MySqlCommandParser.PredicateContext ctx)
        {
            IExpressionSegment left = (IExpressionSegment)Visit(ctx.bitExpr(0));
            IExpressionSegment right = (IExpressionSegment)Visit(ctx.bitExpr(1));
            String @operator = null != ctx.NOT() ? "NOT REGEXP" : "REGEXP";
            String text = ctx.Start.InputStream.GetText(new Interval(ctx.Start.StartIndex, ctx.Stop.StopIndex));
            return new BinaryOperationExpression(ctx.Start.StartIndex, ctx.Stop.StopIndex, left, right,
  @operator, text);
        }

        private BetweenExpression createBetweenSegment(MySqlCommandParser.PredicateContext ctx)
        {
            IExpressionSegment left = (IExpressionSegment)Visit(ctx.bitExpr(0));
            IExpressionSegment between = (IExpressionSegment)Visit(ctx.bitExpr(1));
            IExpressionSegment and = (IExpressionSegment)Visit(ctx.predicate());
            bool not = null != ctx.NOT();
            return new BetweenExpression(ctx.Start.StartIndex, ctx.Stop.StopIndex, left, between, and, not);
        }


        public override IASTNode VisitBitExpr(MySqlCommandParser.BitExprContext ctx)
        {
            if (null != ctx.simpleExpr())
            {
                return Visit(ctx.simpleExpr());
            }

            IExpressionSegment left = (IExpressionSegment)Visit(ctx.GetChild(0));
            IExpressionSegment right = (IExpressionSegment)Visit(ctx.GetChild(2));
            String @operator = ctx.GetChild(1).GetText();
            String text = ctx.Start.InputStream.GetText(new Interval(ctx.Start.StartIndex, ctx.Stop.StopIndex));
            return new BinaryOperationExpression(ctx.Start.StartIndex, ctx.Stop.StopIndex, left, right,
  @operator, text);
        }


        public override IASTNode VisitSimpleExpr(MySqlCommandParser.SimpleExprContext ctx)
        {
            int startIndex = ctx.Start.StartIndex;
            int stopIndex = ctx.Stop.StopIndex;
            if (null != ctx.subquery())
            {
                SubQuerySegment subQuerySegment = new SubQuerySegment(ctx.subquery().Start.StartIndex,
                    ctx.subquery().Stop.StopIndex, (MySqlSelectCommand)Visit(ctx.subquery()));
                if (null != ctx.EXISTS())
                {
                    return new ExistsSubQueryExpression(startIndex, stopIndex, subQuerySegment);
                }

                return new SubQueryExpressionSegment(subQuerySegment);
            }

            if (null != ctx.parameterMarker())
            {
                ParameterMarkerValue parameterMarker = (ParameterMarkerValue)Visit(ctx.parameterMarker());
                ParameterMarkerExpressionSegment segment = new ParameterMarkerExpressionSegment(startIndex, stopIndex,
                    parameterMarker.Value, parameterMarker.ParameterMarkerType,parameterMarker.ParameterName);
                ParameterMarkerSegments.Add(segment);
                return segment;
            }

            if (null != ctx.literals())
            {
                return SqlUtil.CreateLiteralExpression(Visit(ctx.literals()), startIndex, stopIndex,
                    ctx.literals().Start.InputStream.GetText(new Interval(startIndex, stopIndex)));
            }

            if (null != ctx.intervalExpression())
            {
                return Visit(ctx.intervalExpression());
            }

            if (null != ctx.functionCall())
            {
                return Visit(ctx.functionCall());
            }

            if (null != ctx.collateClause())
            {
                ISimpleExpressionSegment collateValueSegment = (ISimpleExpressionSegment)Visit(ctx.collateClause());
                return new CollateExpression(startIndex, stopIndex, collateValueSegment);
            }

            if (null != ctx.columnRef())
            {
                return Visit(ctx.columnRef());
            }

            if (null != ctx.matchExpression())
            {
                return Visit(ctx.matchExpression());
            }

            if (null != ctx.notOperator())
            {
                IASTNode expression = Visit(ctx.simpleExpr(0));
                if (expression is ExistsSubQueryExpression existsSubQueryExpression)
                {
                    existsSubQueryExpression.Not = true;
                    return expression;
                }
                return new NotExpression(startIndex, stopIndex, (IExpressionSegment)expression);
            }

            if (null != ctx.LP_() && 1 == ctx.expr().Length)
            {
                return Visit(ctx.expr(0));
            }

            return VisitRemainSimpleExpr(ctx);
        }


        public override IASTNode VisitColumnRef(MySqlCommandParser.ColumnRefContext ctx)
        {
            int identifierCount = ctx.identifier().Length;
            ColumnSegment result;
            if (1 == identifierCount)
            {
                result = new ColumnSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex,
                    (IdentifierValue)Visit(ctx.identifier(0)));
            }
            else if (2 == identifierCount)
            {
                result = new ColumnSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex,
                    (IdentifierValue)Visit(ctx.identifier(1)));
                result.Owner=new OwnerSegment(ctx.identifier(0).Start.StartIndex, ctx.identifier(0).Stop.StopIndex,
                    (IdentifierValue)Visit(ctx.identifier(0)));
            }
            else
            {
                result = new ColumnSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex,
                    (IdentifierValue)Visit(ctx.identifier(2)));
                OwnerSegment owner = new OwnerSegment(ctx.identifier(1).Start.StartIndex,
                    ctx.identifier(1).Stop.StopIndex, (IdentifierValue)Visit(ctx.identifier(1)));
                owner.Owner=new OwnerSegment(ctx.identifier(0).Start.StartIndex, ctx.identifier(0).Stop.StopIndex,
                    (IdentifierValue)Visit(ctx.identifier(0)));
                result.Owner=owner;
            }

            return result;
        }


        public override IASTNode VisitSubquery(MySqlCommandParser.SubqueryContext ctx)
        {
            return Visit(ctx.queryExpressionParens());
        }


        public override IASTNode VisitQueryExpressionParens(MySqlCommandParser.QueryExpressionParensContext ctx)
        {
            if (null != ctx.queryExpressionParens())
            {
                return Visit(ctx.queryExpressionParens());
            }

            MySqlSelectCommand result = (MySqlSelectCommand)Visit(ctx.queryExpression());
            if (null != ctx.lockClauseList())
            {
                result.Lock=(LockSegment)Visit(ctx.lockClauseList());
            }

            result.ParameterCount=_currentParameterIndex;
            result.ParameterMarkerSegments.AddAll(ParameterMarkerSegments);
            return result;
        }


        public override IASTNode VisitLockClauseList(MySqlCommandParser.LockClauseListContext ctx)
        {
            LockSegment result = new LockSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex);
            foreach (var lockClauseContext in ctx.lockClause())
            {
                if (null != lockClauseContext.tableLockingList())
                {
                    result.Tables
                        .AddAll(GenerateTablesFromTableAliasRefList(lockClauseContext.tableLockingList().tableAliasRefList()));
                }
            }
            return result;
        }


        public override IASTNode VisitQueryExpression(MySqlCommandParser.QueryExpressionContext ctx)
        {
            MySqlSelectCommand result;
            if (null != ctx.queryExpressionBody())
            {
                result = (MySqlSelectCommand)Visit(ctx.queryExpressionBody());
            }
            else
            {
                result = (MySqlSelectCommand)Visit(ctx.queryExpressionParens());
            }

            if (null != ctx.orderByClause())
            {
                result.OrderBy=(OrderBySegment)Visit(ctx.orderByClause());
            }

            if (null != ctx.limitClause())
            {
                result.Limit=(LimitSegment)Visit(ctx.limitClause());
            }

            return result;
        }


        public override IASTNode VisitSelectWithInto(MySqlCommandParser.SelectWithIntoContext ctx)
        {
            if (null != ctx.selectWithInto())
            {
                return Visit(ctx.selectWithInto());
            }

            MySqlSelectCommand result = (MySqlSelectCommand)Visit(ctx.queryExpression());
            if (null != ctx.lockClauseList())
            {
                result.Lock=(LockSegment)Visit(ctx.lockClauseList());
            }

            return result;
        }


        public override IASTNode VisitQueryExpressionBody(MySqlCommandParser.QueryExpressionBodyContext ctx)
        {
            if (1 == ctx.ChildCount && ctx.GetChild(0) is MySqlCommandParser.QueryPrimaryContext) {
                return Visit(ctx.queryPrimary());
            }
            if (null != ctx.queryExpressionBody())
            {
                MySqlSelectCommand result = (MySqlSelectCommand)Visit(ctx.queryExpressionBody());
                CombineSegment combineSegment = (CombineSegment)VisitCombineClause(ctx.combineClause());
                if (result.Combine!=null)
                {
                    result.Combine.SelectCommand.Combine = combineSegment;
                }
                else
                {
                    result.Combine=combineSegment;
                }

                return result;
            }

            MySqlSelectCommand r = (MySqlSelectCommand)Visit(ctx.queryExpressionParens());
            r.Combine=(CombineSegment)VisitCombineClause(ctx.combineClause());
            return r;
        }


        public override IASTNode VisitCombineClause(MySqlCommandParser.CombineClauseContext ctx)
        {
            CombineTypeEnum combineType = (null != ctx.combineOption() && null != ctx.combineOption().ALL())
                ? CombineTypeEnum.UNION_ALL
                : CombineTypeEnum.UNION;
            MySqlSelectCommand statement = null != ctx.queryPrimary()
                ? (MySqlSelectCommand)Visit(ctx.queryPrimary())
                : (MySqlSelectCommand)Visit(ctx.queryExpressionParens());
            return new CombineSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, combineType, statement);
        }


        public override IASTNode VisitQuerySpecification(MySqlCommandParser.QuerySpecificationContext ctx)
        {
            MySqlSelectCommand result = new MySqlSelectCommand();
            result.Projections=(ProjectionsSegment)Visit(ctx.projections());
            if (null != ctx.selectSpecification())
            {
                result.Projections.DistinctRow=IsDistinct(ctx);
            }

            if (null != ctx.fromClause() && null != ctx.fromClause().tableReferences())
            {
                ITableSegment tableSource = (ITableSegment)Visit(ctx.fromClause().tableReferences());
                result.From = tableSource;
            }

            if (null != ctx.whereClause())
            {
                result.Where=(WhereSegment)Visit(ctx.whereClause());
            }

            if (null != ctx.groupByClause())
            {
                result.GroupBy=(GroupBySegment)Visit(ctx.groupByClause());
            }

            if (null != ctx.havingClause())
            {
                result.Having=(HavingSegment)Visit(ctx.havingClause());
            }

            if (null != ctx.windowClause())
            {
                result.Window=(WindowSegment)Visit(ctx.windowClause());
            }

            return result;
        }


        public override IASTNode VisitTableStatement(MySqlCommandParser.TableStatementContext ctx)
        {
            MySqlSelectCommand result = new MySqlSelectCommand();
            result.Table=(SimpleTableSegment)Visit(ctx.tableName());
            return result;
        }


        public override IASTNode VisitWindowClause(MySqlCommandParser.WindowClauseContext ctx)
        {
            return new WindowSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex);
        }


        public override IASTNode VisitHavingClause(MySqlCommandParser.HavingClauseContext ctx)
        {
            IExpressionSegment expr = (IExpressionSegment)Visit(ctx.expr());
            return new HavingSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, expr);
        }


        public override IASTNode VisitIntervalExpression(MySqlCommandParser.IntervalExpressionContext ctx)
        {
            CalculateParameterCount(new []{ctx.intervalValue().expr()});
            return new ExpressionProjectionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, GetOriginalText(ctx),null);
        }


        public override IASTNode VisitFunctionCall(MySqlCommandParser.FunctionCallContext ctx)
        {
            if (null != ctx.aggregationFunction())
            {
                return Visit(ctx.aggregationFunction());
            }

            if (null != ctx.specialFunction())
            {
                return Visit(ctx.specialFunction());
            }

            if (null != ctx.regularFunction())
            {
                return Visit(ctx.regularFunction());
            }

            if (null != ctx.jsonFunction())
            {
                return Visit(ctx.jsonFunction());
            }

            throw new InvalidOperationException(
                "FunctionCallContext must have aggregationFunction, regularFunction, specialFunction or jsonFunction.");
        }


        public override IASTNode VisitAggregationFunction(MySqlCommandParser.AggregationFunctionContext ctx)
        {
            String aggregationType = ctx.aggregationFunctionName().GetText();
            return AggregationType.IsAggregationType(aggregationType)
                ? CreateAggregationSegment(ctx, aggregationType)
                : new ExpressionProjectionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, GetOriginalText(ctx),null);
        }


        public override IASTNode VisitJsonFunction(MySqlCommandParser.JsonFunctionContext ctx)
        {
            MySqlCommandParser.JsonFunctionNameContext functionNameContext = ctx.jsonFunctionName();
            String functionName;
            if (null != functionNameContext)
            {
                functionName = functionNameContext.GetText();
            }
            else if (null != ctx.JSON_SEPARATOR())
            {
                functionName = ctx.JSON_SEPARATOR().GetText();
            }
            else
            {
                functionName = ctx.JSON_UNQUOTED_SEPARATOR().GetText();
            }

            return new FunctionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, functionName, GetOriginalText(ctx));
        }

        private IASTNode CreateAggregationSegment(MySqlCommandParser.AggregationFunctionContext ctx,
            String aggregationType)
        {
            AggregationTypeEnum type = AggregationType.ValueOf(aggregationType.ToUpper());
            String innerExpression = ctx.Start.InputStream
                .GetText(new Interval(ctx.LP_().Symbol.StartIndex, ctx.Stop.StopIndex));
            if (null != ctx.distinct())
            {
                AggregationDistinctProjectionSegment result = new AggregationDistinctProjectionSegment(
                    ctx.Start.StartIndex, ctx.Stop.StopIndex,
                    type, innerExpression, GetDistinctExpression(ctx));
                result.Parameters.AddAll(GetExpressions(ctx));
                return result;
            }

            AggregationProjectionSegment r =
                new AggregationProjectionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, type, innerExpression);
            r.Parameters.AddAll(GetExpressions(ctx));
            return r;
        }

        private IEnumerable<IExpressionSegment> GetExpressions(MySqlCommandParser.AggregationFunctionContext ctx)
        {
            if (null != ctx.expr())
            {
                foreach (var exprContext in ctx.expr())
                {
                    yield return (IExpressionSegment)Visit(exprContext);
                }
            }
        }

        private String GetDistinctExpression(MySqlCommandParser.AggregationFunctionContext ctx)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 3; i < ctx.ChildCount - 1; i++)
            {
                result.Append(ctx.GetChild(i).GetText());
            }

            return result.ToString();
        }


        public override IASTNode VisitSpecialFunction(MySqlCommandParser.SpecialFunctionContext ctx)
        {
            if (null != ctx.groupConcatFunction())
            {
                return Visit(ctx.groupConcatFunction());
            }

            if (null != ctx.windowFunction())
            {
                return Visit(ctx.windowFunction());
            }

            if (null != ctx.castFunction())
            {
                return Visit(ctx.castFunction());
            }

            if (null != ctx.convertFunction())
            {
                return Visit(ctx.convertFunction());
            }

            if (null != ctx.positionFunction())
            {
                return Visit(ctx.positionFunction());
            }

            if (null != ctx.substringFunction())
            {
                return Visit(ctx.substringFunction());
            }

            if (null != ctx.extractFunction())
            {
                return Visit(ctx.extractFunction());
            }

            if (null != ctx.charFunction())
            {
                return Visit(ctx.charFunction());
            }

            if (null != ctx.trimFunction())
            {
                return Visit(ctx.trimFunction());
            }

            if (null != ctx.weightStringFunction())
            {
                return Visit(ctx.weightStringFunction());
            }

            if (null != ctx.valuesFunction())
            {
                return Visit(ctx.valuesFunction());
            }

            if (null != ctx.currentUserFunction())
            {
                return Visit(ctx.currentUserFunction());
            }

            return new FunctionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, GetOriginalText(ctx),
                GetOriginalText(ctx));
        }


        public override IASTNode VisitGroupConcatFunction(MySqlCommandParser.GroupConcatFunctionContext ctx)
        {
            CalculateParameterCount(ctx.expr());
            FunctionSegment result = new FunctionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex,
                ctx.GROUP_CONCAT().GetText(), GetOriginalText(ctx));
            foreach (var exprContext in ctx.expr())
            {
                result.Parameters.Add((IExpressionSegment)Visit(exprContext));
            }
            return result;
        }


        public override IASTNode VisitWindowFunction(MySqlCommandParser.WindowFunctionContext ctx)
        {
            base.VisitWindowFunction(ctx);
            return new FunctionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, ctx.funcName.Text,
                GetOriginalText(ctx));
        }


        public override IASTNode VisitCastFunction(MySqlCommandParser.CastFunctionContext ctx)
        {
            CalculateParameterCount(new[]{ctx.expr()});
            FunctionSegment result = new FunctionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, ctx.CAST().GetText(),
                GetOriginalText(ctx));
            IASTNode exprSegment = Visit(ctx.expr());
            if (exprSegment is ColumnSegment columnSegment) {
                result.Parameters.Add(columnSegment);
            } else if (exprSegment is LiteralExpressionSegment literalExpressionSegment) {
                result.Parameters.Add(literalExpressionSegment);
            }
            result.Parameters.Add((DataTypeSegment)Visit(ctx.dataType()));
            return result;
        }


        public override IASTNode VisitConvertFunction(MySqlCommandParser.ConvertFunctionContext ctx)
        {
            CalculateParameterCount(new []{ctx.expr()});
            return new FunctionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, ctx.CONVERT().GetText(),
                GetOriginalText(ctx));
        }


        public override IASTNode VisitPositionFunction(MySqlCommandParser.PositionFunctionContext ctx)
        {
            CalculateParameterCount(ctx.expr());
            FunctionSegment result = new FunctionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex,
                ctx.POSITION().GetText(), GetOriginalText(ctx));
            result.Parameters.Add((LiteralExpressionSegment)Visit(ctx.expr(0)));
            result.Parameters.Add((LiteralExpressionSegment)Visit(ctx.expr(1)));
            return result;
        }


        public override IASTNode VisitSubstringFunction(MySqlCommandParser.SubstringFunctionContext ctx)
        {
            CalculateParameterCount(new []{ctx.expr()});
            return new FunctionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex,
                null == ctx.SUBSTR() ? ctx.SUBSTRING().GetText() : ctx.SUBSTR().GetText(), GetOriginalText(ctx));
        }


        public override IASTNode VisitExtractFunction(MySqlCommandParser.ExtractFunctionContext ctx)
        {
            CalculateParameterCount(new []{ctx.expr()});
            return new FunctionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, ctx.EXTRACT().GetText(),
                GetOriginalText(ctx));
        }


        public override IASTNode VisitCharFunction(MySqlCommandParser.CharFunctionContext ctx)
        {
            CalculateParameterCount(ctx.expr());
            return new FunctionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, ctx.CHAR().GetText(),
                GetOriginalText(ctx));
        }


        public override IASTNode VisitTrimFunction(MySqlCommandParser.TrimFunctionContext ctx)
        {
            CalculateParameterCount(ctx.expr());
            return new FunctionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, ctx.TRIM().GetText(),
                GetOriginalText(ctx));
        }


        public override IASTNode VisitWeightStringFunction(MySqlCommandParser.WeightStringFunctionContext ctx)
        {
            CalculateParameterCount(new []{ctx.expr()});
            return new FunctionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, ctx.WEIGHT_STRING().GetText(),
                GetOriginalText(ctx));
        }


        public override IASTNode VisitValuesFunction(MySqlCommandParser.ValuesFunctionContext ctx)
        {
            FunctionSegment result = new FunctionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex,
                ctx.VALUES().GetText(), GetOriginalText(ctx));
            if (!ctx.columnRefList().columnRef().IsEmpty())
            {
                ColumnSegment columnSegment = (ColumnSegment)Visit(ctx.columnRefList().columnRef(0));
                result.Parameters.Add(columnSegment);
            }

            return result;
        }


        public override IASTNode VisitCurrentUserFunction(MySqlCommandParser.CurrentUserFunctionContext ctx)
        {
            return new FunctionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, ctx.CURRENT_USER().GetText(),
                GetOriginalText(ctx));
        }


        public override IASTNode VisitRegularFunction(MySqlCommandParser.RegularFunctionContext ctx)
        {
            return null != ctx.completeRegularFunction()
                ? Visit(ctx.completeRegularFunction())
                : Visit(ctx.shorthandRegularFunction());
        }


        public override IASTNode VisitCompleteRegularFunction(MySqlCommandParser.CompleteRegularFunctionContext ctx)
        {
            FunctionSegment result = new FunctionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex,
                ctx.regularFunctionName().GetText(), GetOriginalText(ctx));
            result.Parameters.AddAll(ctx.expr().Select(o=>(IExpressionSegment) Visit(o)));
            return result;
        }


        public override IASTNode VisitShorthandRegularFunction(MySqlCommandParser.ShorthandRegularFunctionContext ctx)
        {
            String text = GetOriginalText(ctx);
            FunctionSegment result;
            if (null != ctx.CURRENT_TIME())
            {
                result = new FunctionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, ctx.CURRENT_TIME().GetText(),
                    text);
                if (null != ctx.NUMBER_())
                {
                    result.Parameters.Add(new LiteralExpressionSegment(ctx.NUMBER_().Symbol.StartIndex,
                        ctx.NUMBER_().Symbol.StopIndex,
                        new NumberLiteralValue(ctx.NUMBER_().GetText())));
                }
            }
            else
            {
                result = new FunctionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, ctx.GetText(), text);
            }

            return result;
        }

        private IASTNode VisitRemainSimpleExpr(MySqlCommandParser.SimpleExprContext ctx)
        {
            if (null != ctx.caseExpression())
            {
                Visit(ctx.caseExpression());
                String text = ctx.Start.InputStream
                    .GetText(new Interval(ctx.Start.StartIndex, ctx.Stop.StopIndex));
                return new CommonExpressionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, text);
            }

            if (null != ctx.BINARY())
            {
                return Visit(ctx.simpleExpr(0));
            }
            foreach (var exprContext in ctx.expr())
            {
                Visit(exprContext);
            }
            foreach (var simpleExprContext in ctx.simpleExpr())
            {
                Visit(simpleExprContext);
            }
            String value = ctx.Start.InputStream.GetText(new Interval(ctx.Start.StartIndex, ctx.Stop.StopIndex));
            return new CommonExpressionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, value);
        }


        public override IASTNode VisitMatchExpression(MySqlCommandParser.MatchExpressionContext ctx)
        {
            Visit(ctx.expr());
            String text = ctx.Start.InputStream.GetText(new Interval(ctx.Start.StartIndex, ctx.Stop.StopIndex));
            return new CommonExpressionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, text);
        }

        // TODO :FIXME, sql case id: insert_with_str_to_date
        private void CalculateParameterCount(IEnumerable<MySqlCommandParser.ExprContext> exprContexts)
        {
            foreach (var exprContext in exprContexts)
            {
                Visit(exprContext);
            }
        }


        public override IASTNode VisitDataType(MySqlCommandParser.DataTypeContext ctx)
        {
            DataTypeSegment result = new DataTypeSegment();
            result.DataTypeName=ctx.dataTypeName.Text;
            result.StartIndex=ctx.Start.StartIndex;
            result.StopIndex=ctx.Stop.StopIndex;
            if (null != ctx.fieldLength())
            {
                DataTypeLengthSegment dataTypeLengthSegment = (DataTypeLengthSegment)Visit(ctx.fieldLength());
                result.DataLength=dataTypeLengthSegment;
            }

            if (null != ctx.precision())
            {
                DataTypeLengthSegment dataTypeLengthSegment = (DataTypeLengthSegment)Visit(ctx.precision());
                result.DataLength=dataTypeLengthSegment;
            }

            return result;
        }


        public override IASTNode VisitFieldLength(MySqlCommandParser.FieldLengthContext ctx)
        {
            DataTypeLengthSegment result = new DataTypeLengthSegment();
            result.StartIndex=ctx.Start.StartIndex;
            result.StopIndex=ctx.Stop.StartIndex;
            result.Precision=int.Parse(ctx.length.Text);
            return result;
        }


        public override IASTNode VisitPrecision(MySqlCommandParser.PrecisionContext ctx)
        {
            DataTypeLengthSegment result = new DataTypeLengthSegment();
            result.StartIndex=ctx.Start.StartIndex;
            result.StopIndex=ctx.Stop.StartIndex;
            ITerminalNode[] numbers = ctx.NUMBER_();
            result.Precision=int.Parse(numbers[0].GetText());
            result.Scale=int.Parse(numbers[1].GetText());
            return result;
        }


        public override IASTNode VisitOrderByClause(MySqlCommandParser.OrderByClauseContext ctx)
        {
            ICollection<OrderByItemSegment> items = new LinkedList<OrderByItemSegment>();
            foreach (var orderByItemContext in ctx.orderByItem())
            {
                items.Add((OrderByItemSegment)Visit(orderByItemContext));
            }
            return new OrderBySegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, items);
        }


        public override IASTNode VisitOrderByItem(MySqlCommandParser.OrderByItemContext ctx)
        {
            OrderDirectionEnum orderDirection;
            if (null != ctx.direction())
            {
                orderDirection = null != ctx.direction().DESC() ? OrderDirectionEnum.DESC : OrderDirectionEnum.ASC;
            }
            else
            {
                orderDirection = OrderDirectionEnum.ASC;
            }

            if (null != ctx.numberLiterals())
            {
                return new IndexOrderByItemSegment(ctx.numberLiterals().Start.StartIndex,
                    ctx.numberLiterals().Stop.StopIndex,
                    (int)SqlUtil.GetExactlyNumber(ctx.numberLiterals().GetText(), 10), orderDirection);
            }
            else
            {
                IASTNode expr = VisitExpr(ctx.expr());
                if (expr is ColumnSegment columnSegment) {
                    return new ColumnOrderByItemSegment(columnSegment, orderDirection);
                } else {
                    return new ExpressionOrderByItemSegment(ctx.expr().Start.StartIndex,
                        ctx.expr().Stop.StopIndex, GetOriginalText(ctx.expr()), orderDirection,
                        (IExpressionSegment)expr);
                }
            }
        }


        public override IASTNode VisitInsert(MySqlCommandParser.InsertContext ctx)
        {
            // TODO :FIXME, since there is no segment for insertValuesClause, InsertStatement is created by sub rule.
            MySqlInsertCommand result;
            if (null != ctx.insertValuesClause())
            {
                result = (MySqlInsertCommand)Visit(ctx.insertValuesClause());
            }
            else if (null != ctx.insertSelectClause())
            {
                result = (MySqlInsertCommand)Visit(ctx.insertSelectClause());
            }
            else
            {
                result = new MySqlInsertCommand();
                result.SetAssignment=(SetAssignmentSegment)Visit(ctx.setAssignmentsClause());
            }

            if (null != ctx.onDuplicateKeyClause())
            {
                result.OnDuplicateKeyColumns=(OnDuplicateKeyColumnsSegment)Visit(ctx.onDuplicateKeyClause());
            }

            result.Table=(SimpleTableSegment)Visit(ctx.tableName());
            result.ParameterCount=_currentParameterIndex;
            result.ParameterMarkerSegments.AddAll(ParameterMarkerSegments);
            return result;
        }


        public override IASTNode VisitInsertSelectClause(MySqlCommandParser.InsertSelectClauseContext ctx)
        {
            MySqlInsertCommand result = new MySqlInsertCommand();
            if (null != ctx.LP_())
            {
                if (null != ctx.fields())
                {
                    result.InsertColumns=new InsertColumnsSegment(ctx.LP_().Symbol.StartIndex,
                        ctx.RP_().Symbol.StopIndex, CreateInsertColumns(ctx.fields()));
                }
                else
                {
                    result.InsertColumns=new InsertColumnsSegment(ctx.LP_().Symbol.StartIndex,
                        ctx.RP_().Symbol.StopIndex,new List<ColumnSegment>(0));
                }
            }
            else
            {
                result.InsertColumns=(new InsertColumnsSegment(ctx.Start.StartIndex - 1, ctx.Start.StartIndex - 1,
                    new List<ColumnSegment>(0)));
            }

            result.InsertSelect=CreateInsertSelectSegment(ctx);
            return result;
        }

        private SubQuerySegment CreateInsertSelectSegment(MySqlCommandParser.InsertSelectClauseContext ctx)
        {
            MySqlSelectCommand selectStatement = (MySqlSelectCommand)Visit(ctx.select());
            return new SubQuerySegment(ctx.select().Start.StartIndex, ctx.select().Stop.StopIndex, selectStatement);
        }


        public override IASTNode VisitInsertValuesClause(MySqlCommandParser.InsertValuesClauseContext ctx)
        {
            MySqlInsertCommand result = new MySqlInsertCommand();
            if (null != ctx.LP_())
            {
                if (null != ctx.fields())
                {
                    result.InsertColumns=new InsertColumnsSegment(ctx.LP_().Symbol.StartIndex,
                        ctx.RP_().Symbol.StopIndex, CreateInsertColumns(ctx.fields()));
                }
                else
                {
                    result.InsertColumns=new InsertColumnsSegment(ctx.LP_().Symbol.StartIndex,
                        ctx.RP_().Symbol.StopIndex, new List<ColumnSegment>(0));
                }
            }
            else
            {
                result.InsertColumns=new InsertColumnsSegment(ctx.Start.StartIndex - 1, ctx.Start.StartIndex - 1,
                    new List<ColumnSegment>(0));
            }

            result.Values.AddAll(CreateInsertValuesSegments(ctx.assignmentValues()));
            return result;
        }

        private IEnumerable<InsertValuesSegment> CreateInsertValuesSegments(
            MySqlCommandParser.AssignmentValuesContext[] assignmentValuesContexts)
        {
            foreach (var assignmentValuesContext in assignmentValuesContexts)
            {
                var insertValuesSegment = (InsertValuesSegment)Visit(assignmentValuesContext);
                yield return insertValuesSegment;
            }
        }


        public override IASTNode VisitOnDuplicateKeyClause(MySqlCommandParser.OnDuplicateKeyClauseContext ctx)
        {
            var columns = ctx.assignment().Select(o=>(AssignmentSegment)Visit(o)).ToList();
            return new OnDuplicateKeyColumnsSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, columns);
        }


        public override IASTNode VisitReplace(MySqlCommandParser.ReplaceContext ctx)
        {
            // TODO :FIXME, since there is no segment for replaceValuesClause, ReplaceStatement is created by sub rule.
            MySqlInsertCommand result;
            if (null != ctx.replaceValuesClause())
            {
                result = (MySqlInsertCommand)Visit(ctx.replaceValuesClause());
            }
            else if (null != ctx.replaceSelectClause())
            {
                result = (MySqlInsertCommand)Visit(ctx.replaceSelectClause());
            }
            else
            {
                result = new MySqlInsertCommand();
                result.SetAssignment=(SetAssignmentSegment)Visit(ctx.setAssignmentsClause());
            }

            result.Table=(SimpleTableSegment)Visit(ctx.tableName());
            result.ParameterCount=_currentParameterIndex;
            result.ParameterMarkerSegments.AddAll(ParameterMarkerSegments);
            return result;
        }


        public override IASTNode VisitReplaceSelectClause(MySqlCommandParser.ReplaceSelectClauseContext ctx)
        {
            MySqlInsertCommand result = new MySqlInsertCommand();
            if (null != ctx.LP_())
            {
                if (null != ctx.fields())
                {
                    result.InsertColumns=new InsertColumnsSegment(ctx.LP_().Symbol.StartIndex,
                        ctx.RP_().Symbol.StopIndex, CreateInsertColumns(ctx.fields()));
                }
                else
                {
                    result.InsertColumns=new InsertColumnsSegment(ctx.LP_().Symbol.StartIndex,
                        ctx.RP_().Symbol.StopIndex, new List<ColumnSegment>(0));
                }
            }
            else
            {
                result.InsertColumns=new InsertColumnsSegment(ctx.Start.StartIndex - 1, ctx.Start.StartIndex - 1,
                    new List<ColumnSegment>(0));
            }

            result.InsertSelect=CreateReplaceSelectSegment(ctx);
            return result;
        }

        private SubQuerySegment CreateReplaceSelectSegment(MySqlCommandParser.ReplaceSelectClauseContext ctx)
        {
            MySqlSelectCommand selectStatement = (MySqlSelectCommand)Visit(ctx.select());
            return new SubQuerySegment(ctx.select().Start.StartIndex, ctx.select().Stop.StopIndex, selectStatement);
        }


        public override IASTNode VisitReplaceValuesClause(MySqlCommandParser.ReplaceValuesClauseContext ctx)
        {
            MySqlInsertCommand result = new MySqlInsertCommand();
            if (null != ctx.LP_())
            {
                if (null != ctx.fields())
                {
                    result.InsertColumns=new InsertColumnsSegment(ctx.LP_().Symbol.StartIndex,
                        ctx.RP_().Symbol.StopIndex, CreateInsertColumns(ctx.fields()));
                }
                else
                {
                    result.InsertColumns=new InsertColumnsSegment(ctx.LP_().Symbol.StartIndex,
                        ctx.RP_().Symbol.StopIndex, new List<ColumnSegment>(0));
                }
            }
            else
            {
                result.InsertColumns=new InsertColumnsSegment(ctx.Start.StartIndex - 1, ctx.Start.StartIndex - 1,
                    new List<ColumnSegment>(0));
            }

            result.Values.AddAll(CreateReplaceValuesSegments(ctx.assignmentValues()));
            return result;
        }

        private ICollection<ColumnSegment> CreateInsertColumns(MySqlCommandParser.FieldsContext fields)
        {
            return fields.insertIdentifier().Select(o => (ColumnSegment)Visit(o)).ToList();
        }

        private IEnumerable<InsertValuesSegment> CreateReplaceValuesSegments(
            MySqlCommandParser.AssignmentValuesContext[] assignmentValuesContexts)
        {
            foreach (var assignmentValuesContext in assignmentValuesContexts)
            {
                var insertValuesSegment = (InsertValuesSegment)Visit(assignmentValuesContext);
                yield return insertValuesSegment;
            }
        }


        public override IASTNode VisitUpdate(MySqlCommandParser.UpdateContext ctx)
        {
            MySqlUpdateCommand result = new MySqlUpdateCommand();
            ITableSegment tableSegment = (ITableSegment)Visit(ctx.tableReferences());
            result.Table=tableSegment;
            result.SetAssignment=(SetAssignmentSegment)Visit(ctx.setAssignmentsClause());
            if (null != ctx.whereClause())
            {
                result.Where=(WhereSegment)Visit(ctx.whereClause());
            }

            if (null != ctx.orderByClause())
            {
                result.OrderBy=(OrderBySegment)Visit(ctx.orderByClause());
            }

            if (null != ctx.limitClause())
            {
                result.Limit=(LimitSegment)Visit(ctx.limitClause());
            }

            result.ParameterCount=_currentParameterIndex;
            result.ParameterMarkerSegments.AddAll(ParameterMarkerSegments);
            return result;
        }


        public override IASTNode VisitSetAssignmentsClause(MySqlCommandParser.SetAssignmentsClauseContext ctx)
        {
            var assignments = ctx.assignment().Select(o=>(AssignmentSegment)Visit(o)).ToList();
            return new SetAssignmentSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, assignments);
        }


        public override IASTNode VisitAssignmentValues(MySqlCommandParser.AssignmentValuesContext ctx)
        {
            var segments = ctx.assignmentValue().Select(o=>(IExpressionSegment)Visit(o)).ToList();
            return new InsertValuesSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, segments);
        }


        public override IASTNode VisitAssignment(MySqlCommandParser.AssignmentContext ctx)
        {
            ColumnSegment column = (ColumnSegment)Visit(ctx.columnRef());
            IExpressionSegment value = (IExpressionSegment)Visit(ctx.assignmentValue());
            List<ColumnSegment> columnSegments = new List<ColumnSegment>() { column };
            return new ColumnAssignmentSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, columnSegments, value);
        }


        public override IASTNode VisitAssignmentValue(MySqlCommandParser.AssignmentValueContext ctx)
        {
            MySqlCommandParser.ExprContext expr = ctx.expr();
            if (null != expr)
            {
                IASTNode result = Visit(expr);
                if (result is ColumnSegment columnSegment) {
                    return new CommonExpressionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, ctx.GetText());
                } else {
                    return result;
                }
            }

            return new CommonExpressionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, ctx.GetText());
        }


        public override IASTNode VisitBlobValue(MySqlCommandParser.BlobValueContext ctx)
        {
            return new StringLiteralValue(ctx.string_().GetText());
        }


        public override IASTNode VisitDelete(MySqlCommandParser.DeleteContext ctx)
        {
            ITableSegment table;
            if (null != ctx.multipleTablesClause())
            {
                table=(ITableSegment)Visit(ctx.multipleTablesClause());
            }
            else
            {
                table=(ITableSegment)Visit(ctx.singleTableClause());
            }
            MySqlDeleteCommand result = new MySqlDeleteCommand(table);

            if (null != ctx.whereClause())
            {
                result.Where=(WhereSegment)Visit(ctx.whereClause());
            }

            if (null != ctx.orderByClause())
            {
                result.OrderBy=(OrderBySegment)Visit(ctx.orderByClause());
            }

            if (null != ctx.limitClause())
            {
                result.Limit=(LimitSegment)Visit(ctx.limitClause());
            }

            result.ParameterCount=_currentParameterIndex;
            result.ParameterMarkerSegments.AddAll(ParameterMarkerSegments);
            return result;
        }


        public override IASTNode VisitSingleTableClause(MySqlCommandParser.SingleTableClauseContext ctx)
        {
            SimpleTableSegment result = (SimpleTableSegment)Visit(ctx.tableName());
            if (null != ctx.alias())
            {
                result.SetAlias((AliasSegment)Visit(ctx.alias()));
            }

            return result;
        }


        public override IASTNode VisitMultipleTablesClause(MySqlCommandParser.MultipleTablesClauseContext ctx)
        {
            DeleteMultiTableSegment result = new DeleteMultiTableSegment();
            ITableSegment relateTableSource = (ITableSegment)Visit(ctx.tableReferences());
            result.RelationTable=relateTableSource;
            result.ActualDeleteTables=GenerateTablesFromTableAliasRefList(ctx.tableAliasRefList());
            return result;
        }

        private List<SimpleTableSegment> GenerateTablesFromTableAliasRefList(
            MySqlCommandParser.TableAliasRefListContext ctx)
        {
            return ctx.tableIdentOptWild().Select(o=>(SimpleTableSegment)Visit(o.tableName())).ToList();
        }


        public override IASTNode VisitSelect(MySqlCommandParser.SelectContext ctx)
        {
            // TODO :Unsupported for withClause.
            MySqlSelectCommand result;
            if (null != ctx.queryExpression())
            {
                result = (MySqlSelectCommand)Visit(ctx.queryExpression());
                if (null != ctx.lockClauseList())
                {
                    result.Lock=(LockSegment)Visit(ctx.lockClauseList());
                }
            }
            else if (null != ctx.selectWithInto())
            {
                result = (MySqlSelectCommand)Visit(ctx.selectWithInto());
            }
            else
            {
                result = (MySqlSelectCommand)Visit(ctx.GetChild(0));
            }

            result.ParameterCount=_currentParameterIndex;
            result.ParameterMarkerSegments.AddAll(ParameterMarkerSegments);
            return result;
        }

        private bool IsDistinct(MySqlCommandParser.QuerySpecificationContext ctx)
        {
            foreach (var selectSpecificationContext in ctx.selectSpecification())
            {
                if (((BooleanLiteralValue)Visit(selectSpecificationContext)).Value)
                {
                    return true;
                }
            }
            return false;
        }


        public override IASTNode VisitSelectSpecification(MySqlCommandParser.SelectSpecificationContext ctx)
        {
            if (null != ctx.duplicateSpecification())
            {
                return Visit(ctx.duplicateSpecification());
            }

            return new BooleanLiteralValue(false);
        }


        public override IASTNode VisitDuplicateSpecification(MySqlCommandParser.DuplicateSpecificationContext ctx)
        {
            String text = ctx.GetText();
            if ("DISTINCT".EqualsIgnoreCase(text) || "DISTINCTROW".EqualsIgnoreCase(text))
            {
                return new BooleanLiteralValue(true);
            }

            return new BooleanLiteralValue(false);
        }


        public override IASTNode VisitProjections(MySqlCommandParser.ProjectionsContext ctx)
        {
            ICollection<IProjectionSegment> projections = new LinkedList<IProjectionSegment>();
            if (null != ctx.unqualifiedShorthand())
            {
                projections.Add(new ShorthandProjectionSegment(ctx.unqualifiedShorthand().Start.StartIndex,
                    ctx.unqualifiedShorthand().Stop.StopIndex));
            }
            foreach (var projectionContext in ctx.projection())
            {
                projections.Add((IProjectionSegment)Visit(projectionContext));
            }
            ProjectionsSegment result = new ProjectionsSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex);
            result.Projections.AddAll(projections);
            return result;
        }


        public override IASTNode VisitProjection(MySqlCommandParser.ProjectionContext ctx)
        {
            // FIXME :The stop index of project is the stop index of projection, instead of alias.
            if (null != ctx.qualifiedShorthand())
            {
                return createShorthandProjection(ctx.qualifiedShorthand());
            }

            AliasSegment? alias = null == ctx.alias() ? null : (AliasSegment)Visit(ctx.alias());
            IASTNode exprProjection = Visit(ctx.expr());
            if (exprProjection is ColumnSegment columnSegment) {
                ColumnProjectionSegment result = new ColumnProjectionSegment((ColumnSegment)exprProjection);
                result.SetAlias(alias);
                return result;
            }
            if (exprProjection is SubQuerySegment subQuerySegment) {
                String text = ctx.Start.InputStream
                    .GetText(new Interval(subQuerySegment.StartIndex, subQuerySegment.StopIndex));
                SubQueryProjectionSegment result = new SubQueryProjectionSegment((SubQuerySegment)exprProjection, text);
                result.SetAlias(alias);
                return result;
            }
            if (exprProjection is ExistsSubQueryExpression existsSubQueryExpression) {
                String text = ctx.Start.InputStream.GetText(new Interval(existsSubQueryExpression.StartIndex,
                    existsSubQueryExpression.StopIndex));
                SubQueryProjectionSegment result =
                    new SubQueryProjectionSegment(((ExistsSubQueryExpression)exprProjection).SubQuery, text);
                result.SetAlias(alias);
                return result;
            }
            return CreateProjection(ctx, alias, exprProjection);
        }

        private ShorthandProjectionSegment createShorthandProjection(
            MySqlCommandParser.QualifiedShorthandContext shorthand)
        {
            ShorthandProjectionSegment result =
                new ShorthandProjectionSegment(shorthand.Start.StartIndex, shorthand.Stop.StopIndex);
            MySqlCommandParser.IdentifierContext identifier =
                shorthand.identifier()[shorthand.identifier().Length - 1];
            OwnerSegment owner = new OwnerSegment(identifier.Start.StartIndex, identifier.Stop.StopIndex,
                new IdentifierValue(identifier.GetText()));
            result.Owner=owner;
            if (shorthand.identifier().Length > 1)
            {
                MySqlCommandParser.IdentifierContext schemaIdentifier = shorthand.identifier()[0];
                owner.Owner=new OwnerSegment(schemaIdentifier.Start.StartIndex, schemaIdentifier.Stop.StopIndex,
                    new IdentifierValue(schemaIdentifier.GetText()));
            }

            return result;
        }


        public override IASTNode VisitAlias(MySqlCommandParser.AliasContext ctx)
        {
            return new AliasSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex,
                new IdentifierValue(ctx.textOrIdentifier().GetText()));
        }

        private IASTNode CreateProjection(MySqlCommandParser.ProjectionContext ctx, AliasSegment? alias,
            IASTNode projection)
        {
            if (projection is AggregationProjectionSegment aggregationProjectionSegment) {
                aggregationProjectionSegment.SetAlias(alias);
                return projection;
            }
            if (projection is ExpressionProjectionSegment expressionProjectionSegment) {
                expressionProjectionSegment.SetAlias(alias);
                return projection;
            }
            if (projection is FunctionSegment functionSegment) {
                ExpressionProjectionSegment result1 = new ExpressionProjectionSegment(functionSegment.StartIndex,
                    functionSegment.StopIndex, functionSegment.Text, functionSegment);
                result1.SetAlias(alias);
                return result1;
            }
            if (projection is CommonExpressionSegment commonExpressionSegment) {
                ExpressionProjectionSegment result2 = new ExpressionProjectionSegment(commonExpressionSegment.StartIndex,
                    commonExpressionSegment.StopIndex, commonExpressionSegment.Text, commonExpressionSegment);
                result2.SetAlias(alias);
                return result2;
            }
            // FIXME :For DISTINCT()
            if (projection is ColumnSegment columnSegment) {
                ExpressionProjectionSegment result3 = new ExpressionProjectionSegment(ctx.Start.StartIndex,
                    ctx.Stop.StopIndex, GetOriginalText(ctx), columnSegment);
                result3.SetAlias(alias);
                return result3;
            }
            if (projection is SubQueryExpressionSegment subQueryExpressionSegment) {
                String text = ctx.Start.InputStream.GetText(new Interval(subQueryExpressionSegment.StartIndex,
                    subQueryExpressionSegment.StopIndex));
                SubQueryProjectionSegment result4 =
                    new SubQueryProjectionSegment(subQueryExpressionSegment.SubQuery, text);
                result4.SetAlias(alias);
                return result4;
            }
            if (projection is BinaryOperationExpression) {
                int startIndex = ((BinaryOperationExpression)projection).StartIndex;
                int stopIndex = alias?.StopIndex ?? ((BinaryOperationExpression)projection).StopIndex;
                ExpressionProjectionSegment result5 = new ExpressionProjectionSegment(startIndex, stopIndex,
                    ((BinaryOperationExpression)projection).Text, (BinaryOperationExpression)projection);
                result5.SetAlias(alias);
                return result5;
            }
            if (projection is ParameterMarkerExpressionSegment parameterMarkerExpressionSegment) {
                parameterMarkerExpressionSegment.SetAlias(alias);
                return parameterMarkerExpressionSegment;
            }
            LiteralExpressionSegment column = (LiteralExpressionSegment)projection;
            ExpressionProjectionSegment result = null == alias
                ? new ExpressionProjectionSegment(column.StartIndex, column.StopIndex,
                    $"{column.Literals}", column)
                : new ExpressionProjectionSegment(column.StartIndex, ctx.alias().Stop.StopIndex,
                    $"{column.Literals}", column);
            result.SetAlias(alias);
            return result;
        }


        public override IASTNode VisitFromClause(MySqlCommandParser.FromClauseContext ctx)
        {
            return Visit(ctx.tableReferences());
        }


        public override IASTNode VisitTableReferences(MySqlCommandParser.TableReferencesContext ctx)
        {
            ITableSegment result = (ITableSegment)Visit(ctx.tableReference(0));
            if (ctx.tableReference().Length > 1)
            {
                for (int i = 1; i < ctx.tableReference().Length; i++)
                {
                    result = GenerateJoinTableSourceFromEscapedTableReference(ctx.tableReference(i), result);
                }
            }

            return result;
        }

        private JoinTableSegment GenerateJoinTableSourceFromEscapedTableReference(
            MySqlCommandParser.TableReferenceContext ctx, ITableSegment tableSegment)
        {
            JoinTableSegment result = new JoinTableSegment();
            result.StartIndex=tableSegment.StartIndex;
            result.StopIndex=ctx.Stop.StopIndex;
            result.Left=tableSegment;
            result.JoinType = nameof(JoinTypeEnum.COMMA);
            result.Right=(ITableSegment)Visit(ctx);
            return result;
        }


        public override IASTNode VisitEscapedTableReference(MySqlCommandParser.EscapedTableReferenceContext ctx)
        {
            ITableSegment left= (ITableSegment)Visit(ctx.tableFactor());
            foreach (var joinedTableContext in ctx.joinedTable())
            {
                left = VisitJoinedTable(joinedTableContext, left);
            }
            return left;
        }


        public override IASTNode VisitTableReference(MySqlCommandParser.TableReferenceContext ctx)
        {
            ITableSegment left= null != ctx.tableFactor()
                ? (ITableSegment)Visit(ctx.tableFactor())
                : (ITableSegment)Visit(ctx.escapedTableReference());
            foreach (var joinedTableContext in ctx.joinedTable())
            {
                left = VisitJoinedTable(joinedTableContext, left);
            }
            return left;
        }


        public override IASTNode VisitTableFactor(MySqlCommandParser.TableFactorContext ctx)
        {
            if (null != ctx.subquery())
            {
                MySqlSelectCommand subQuery = (MySqlSelectCommand)Visit(ctx.subquery());
                SubQuerySegment subQuerySegment = new SubQuerySegment(ctx.subquery().Start.StartIndex,
                    ctx.subquery().Stop.StopIndex, subQuery);
                SubQueryTableSegment result = new SubQueryTableSegment(subQuerySegment);
                if (null != ctx.alias())
                {
                    result.SetAlias((AliasSegment)Visit(ctx.alias()));
                }

                return result;
            }

            if (null != ctx.tableName())
            {
                SimpleTableSegment result = (SimpleTableSegment)Visit(ctx.tableName());
                if (null != ctx.alias())
                {
                    result.SetAlias((AliasSegment)Visit(ctx.alias()));
                }

                return result;
            }

            return Visit(ctx.tableReferences());
        }

        private JoinTableSegment VisitJoinedTable(MySqlCommandParser.JoinedTableContext ctx, ITableSegment tableSegment)
        {
            JoinTableSegment result = new JoinTableSegment();
            result.Left=tableSegment;
            result.StartIndex=tableSegment.StartIndex;
            result.StopIndex=ctx.Stop.StopIndex;
            result.JoinType=GetJoinType(ctx);
            ITableSegment right = null != ctx.tableFactor()
                ? (ITableSegment)Visit(ctx.tableFactor())
                : (ITableSegment)Visit(ctx.tableReference());
            result.Right=right;
            return null != ctx.joinSpecification() ? VisitJoinSpecification(ctx.joinSpecification(), result) : result;
        }

        private String GetJoinType(MySqlCommandParser.JoinedTableContext ctx)
        {
            if (null != ctx.innerJoinType())
            {
                return nameof(JoinTypeEnum.INNER);
            }

            if (null != ctx.outerJoinType())
            {
                return ctx.outerJoinType().LEFT() != null ? nameof(JoinTypeEnum.LEFT) : nameof(JoinTypeEnum.RIGHT);
            }

            if (null != ctx.naturalJoinType())
            {
                return GetNaturalJoinType(ctx.naturalJoinType());
            }

            return nameof(JoinTypeEnum.COMMA);
        }

        private static String GetNaturalJoinType(MySqlCommandParser.NaturalJoinTypeContext ctx)
        {
            if (null != ctx.LEFT())
            {
                return nameof(JoinTypeEnum.LEFT);
            }
            else if (null != ctx.RIGHT())
            {
                return nameof(JoinTypeEnum.RIGHT);
            }
            else
            {
                return nameof(JoinTypeEnum.INNER);
            }
        }

        private JoinTableSegment VisitJoinSpecification(MySqlCommandParser.JoinSpecificationContext ctx,
            JoinTableSegment result)
        {
            if (null != ctx.expr())
            {
                IExpressionSegment condition = (IExpressionSegment)Visit(ctx.expr());
                result.Condition=condition;
            }

            if (null != ctx.USING())
            {
                result.Using=ctx.columnNames().columnName().Select(o=>(ColumnSegment) Visit(o)).ToList();
            }

            return result;
        }


        public override IASTNode VisitWhereClause(MySqlCommandParser.WhereClauseContext ctx)
        {
            IASTNode segment = Visit(ctx.expr());
            return new WhereSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, (IExpressionSegment)segment);
        }


        public override IASTNode VisitGroupByClause(MySqlCommandParser.GroupByClauseContext ctx)
        {
            var orderByItems = ctx.orderByItem().Select(o=>(OrderByItemSegment)Visit(o)).ToList();
            return new GroupBySegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, orderByItems);
        }


        public override IASTNode VisitLimitClause(MySqlCommandParser.LimitClauseContext ctx)
        {
            if (null == ctx.limitOffset())
            {
                return new LimitSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, null,
                    (IPaginationValueSegment)Visit(ctx.limitRowCount()));
            }

            IPaginationValueSegment rowCount;
            IPaginationValueSegment offset;
            if (null != ctx.OFFSET())
            {
                rowCount = (IPaginationValueSegment)Visit(ctx.limitRowCount());
                offset = (IPaginationValueSegment)Visit(ctx.limitOffset());
            }
            else
            {
                offset = (IPaginationValueSegment)Visit(ctx.limitOffset());
                rowCount = (IPaginationValueSegment)Visit(ctx.limitRowCount());
            }

            return new LimitSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, offset, rowCount);
        }


        public override IASTNode VisitLimitRowCount(MySqlCommandParser.LimitRowCountContext ctx)
        {
            if (null != ctx.numberLiterals())
            {
                return new NumberLiteralLimitValueSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex,
                    (long)((NumberLiteralValue)Visit(ctx.numberLiterals())).Value);
            }

            var parameterMarkerValue = (ParameterMarkerValue)Visit(ctx.parameterMarker());
            IParameterMarkerSegment result = new ParameterMarkerLimitValueSegment(ctx.Start.StartIndex,
                ctx.Stop.StopIndex,parameterMarkerValue.Value,parameterMarkerValue.ParameterName);
            ParameterMarkerSegments.Add(result);
            return result;
        }


        public override IASTNode VisitConstraintName(MySqlCommandParser.ConstraintNameContext ctx)
        {
            return new ConstraintSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex,
                (IdentifierValue)Visit(ctx.identifier()));
        }


        public override IASTNode VisitLimitOffset(MySqlCommandParser.LimitOffsetContext ctx)
        {
            if (null != ctx.numberLiterals())
            {
                return new NumberLiteralLimitValueSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex,
                    (long)((NumberLiteralValue)Visit(ctx.numberLiterals())).Value);
            }

            var parameterMarkerValue = (ParameterMarkerValue)Visit(ctx.parameterMarker());
            IParameterMarkerSegment result = new ParameterMarkerLimitValueSegment(ctx.Start.StartIndex,
                ctx.Stop.StopIndex,parameterMarkerValue.Value,parameterMarkerValue.ParameterName);
            ParameterMarkerSegments.Add(result);
            return result;
        }


        public override IASTNode VisitCollateClause(MySqlCommandParser.CollateClauseContext ctx)
        {
            if (null != ctx.collationName())
            {
                return new LiteralExpressionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex,
                    ctx.collationName().textOrIdentifier().GetText());
            }

            var parameterMarkerValue = (ParameterMarkerValue)Visit(ctx.parameterMarker());
            ParameterMarkerExpressionSegment segment = new ParameterMarkerExpressionSegment(ctx.Start.StartIndex,
                ctx.Stop.StopIndex,
                parameterMarkerValue.Value,parameterMarkerValue.ParameterName);
            ParameterMarkerSegments.Add(segment);
            return segment;
        }

        /**
     * Get original text.
     *
     * @param ctx context
     * @return original text
     */
        private String GetOriginalText(ParserRuleContext ctx)
        {
            return ctx.Start.InputStream.GetText(new Interval(ctx.Start.StartIndex, ctx.Stop.StopIndex));
        }
    }
}