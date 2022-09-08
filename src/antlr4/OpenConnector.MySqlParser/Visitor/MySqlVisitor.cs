using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParser.Command.DML;
using OpenConnector.CommandParser.Constant;
using OpenConnector.CommandParser.Predicate;
using OpenConnector.CommandParser.Segment.DDL.Index;
using OpenConnector.CommandParser.Segment.DML.Column;
using OpenConnector.CommandParser.Segment.DML.Expr;
using OpenConnector.CommandParser.Segment.DML.Expr.Complex;
using OpenConnector.CommandParser.Segment.DML.Expr.Simple;
using OpenConnector.CommandParser.Segment.DML.Expr.SubQuery;
using OpenConnector.CommandParser.Segment.DML.Item;
using OpenConnector.CommandParser.Segment.DML.Order;
using OpenConnector.CommandParser.Segment.DML.Order.Item;
using OpenConnector.CommandParser.Segment.DML.Predicate;
using OpenConnector.CommandParser.Segment.DML.Predicate.Value;
using OpenConnector.CommandParser.Segment.Generic;
using OpenConnector.CommandParser.Segment.Generic.Table;
using OpenConnector.CommandParser.Util;
using OpenConnector.CommandParser.Value.Collection;
using OpenConnector.CommandParser.Value.Identifier;
using OpenConnector.CommandParser.Value.KeyWord;
using OpenConnector.CommandParser.Value.Literal.Impl;
using OpenConnector.CommandParser.Value.ParameterMaker;
using OpenConnector.Exceptions;


namespace OpenConnector.MySqlParser.Visitor
{
    /// <summary>
    /// 
    /// </summary>
    /// Author: xjm
    /// Created: 2022/5/10 8:09:26
    /// Email: 326308290@qq.com
    public  class MySqlVisitor : MySqlCommandBaseVisitor<IASTNode>
    {
        private int _currentParameterIndex;

        public int GetCurrentParameterIndex()
        {
            return _currentParameterIndex;
        }
        public override IASTNode VisitParameterMarker(MySqlCommandParser.ParameterMarkerContext context)
        {
            return new ParameterMarkerValue(_currentParameterIndex++);
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
            if (null != ctx.dateTimeLiterals())
            {
                return Visit(ctx.dateTimeLiterals());
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
            throw new ShardingException("Literals must have string, number, dateTime, hex, bit, boolean or null.");
        }

        public override IASTNode VisitStringLiterals(MySqlCommandParser.StringLiteralsContext ctx)
        {
            return new StringLiteralValue(ctx.GetText());
        }

        public override IASTNode VisitNumberLiterals(MySqlCommandParser.NumberLiteralsContext ctx)
        {
            return new NumberLiteralValue(ctx.GetText());
        }

        public override IASTNode VisitDateTimeLiterals(MySqlCommandParser.DateTimeLiteralsContext ctx)
        {
            // TODO deal with dateTimeLiterals
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
            MySqlCommandParser.UnreservedWordContext unreservedWord = ctx.unreservedWord();
            return null != unreservedWord ? Visit(unreservedWord) : new IdentifierValue(ctx.GetText());
        }


        public override IASTNode VisitUnreservedWord(MySqlCommandParser.UnreservedWordContext ctx)
        {
            return new IdentifierValue(ctx.GetText());
        }

        public override IASTNode VisitSchemaName(MySqlCommandParser.SchemaNameContext ctx)
        {
            return Visit(ctx.identifier());
        }

        public override IASTNode VisitTableName(MySqlCommandParser.TableNameContext ctx)
        {
            SimpleTableSegment result = new SimpleTableSegment(new TableNameSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, (IdentifierValue)Visit(ctx.name())));
            MySqlCommandParser.OwnerContext owner = ctx.owner();
            if (null != owner)
            {
                result.SetOwner(new OwnerSegment(owner.Start.StartIndex, owner.Stop.StopIndex, (IdentifierValue)Visit(owner.identifier())));
            }
            return result;
        }

        public override IASTNode VisitColumnName(MySqlCommandParser.ColumnNameContext ctx)
        {
            ColumnSegment result = new ColumnSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, (IdentifierValue)Visit(ctx.name()));
            MySqlCommandParser.OwnerContext owner = ctx.owner();
            if (null != owner)
            {
                result.SetOwner(new OwnerSegment(owner.Start.StartIndex, owner.Stop.StopIndex, (IdentifierValue)Visit(owner.identifier())));
            }
            return result;
        }

        public override IASTNode VisitIndexName(MySqlCommandParser.IndexNameContext ctx)
        {
            return new IndexSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, (IdentifierValue)Visit(ctx.identifier()));
        }

        public override IASTNode VisitTableNames(MySqlCommandParser.TableNamesContext ctx)
        {
            CollectionValue<SimpleTableSegment> result = new CollectionValue<SimpleTableSegment>();
            foreach (var each in ctx.tableName())
            {
                result.GetValue().Add((SimpleTableSegment)Visit(each));
            }

            return result;
        }

        public override IASTNode VisitColumnNames(MySqlCommandParser.ColumnNamesContext ctx)
        {
            CollectionValue<ColumnSegment> result = new CollectionValue<ColumnSegment>();
            foreach (var each in ctx.columnName())
            {
                result.GetValue().Add((ColumnSegment)Visit(each));
            }
            return result;
        }

        public override IASTNode VisitExpr(MySqlCommandParser.ExprContext ctx)
        {
            if (null != ctx.booleanPrimary())
            {
                return Visit(ctx.booleanPrimary());
            }
            if (null != ctx.logicalOperator())
            {
                return new PredicateBuilder(Visit(ctx.expr(0)), Visit(ctx.expr(1)), ctx.logicalOperator().GetText()).MergePredicate();
            }
            // TODO deal with XOR
            return Visit(ctx.expr()[0]);
        }

        public override IASTNode VisitBooleanPrimary(MySqlCommandParser.BooleanPrimaryContext ctx)
        {
            if (null != ctx.comparisonOperator() || null != ctx.SAFE_EQ_())
            {
                return CreateCompareSegment(ctx);
            }
            if (null != ctx.predicate())
            {
                return Visit(ctx.predicate());
            }
            if (null != ctx.subquery())
            {
                return new SubQuerySegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, (SelectCommand)Visit(ctx.subquery()));
            }
            //TODO deal with IS NOT? (TRUE | FALSE | UNKNOWN | NULL)
            return new CommonExpressionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, ctx.GetText());
        }

        private IASTNode CreateCompareSegment(MySqlCommandParser.BooleanPrimaryContext ctx)
        {
            IASTNode leftValue = Visit(ctx.booleanPrimary());
            if (!(leftValue is ColumnSegment))
            {
                return leftValue;
            }
            IPredicateRightValue rightValue = (IPredicateRightValue)CreatePredicateRightValue(ctx);
            return new PredicateSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, (ColumnSegment)leftValue, rightValue);
        }

        private IASTNode CreatePredicateRightValue(MySqlCommandParser.BooleanPrimaryContext ctx)
        {
            if (null != ctx.subquery())
            {
                new SubQuerySegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, (SelectCommand)Visit(ctx.subquery()));
            }
            IASTNode rightValue = Visit(ctx.predicate());
            return CreatePredicateRightValue(ctx, rightValue);
        }

        private IASTNode CreatePredicateRightValue(MySqlCommandParser.BooleanPrimaryContext ctx, IASTNode rightValue)
        {
            if (rightValue is ColumnSegment)
            {
                return rightValue;
            }
            return rightValue is SubQuerySegment subQuerySegment ? new PredicateCompareRightValue(ctx.comparisonOperator().GetText(), new SubQueryExpressionSegment(subQuerySegment))
                    : new PredicateCompareRightValue(ctx.comparisonOperator().GetText(), (IExpressionSegment)rightValue);
        }

        public override IASTNode VisitPredicate(MySqlCommandParser.PredicateContext ctx)
        {
            if (null != ctx.IN() && null == ctx.NOT())
            {
                return CreateInSegment(ctx);
            }
            if (null != ctx.BETWEEN() && null == ctx.NOT())
            {
                return CreateBetweenSegment(ctx);
            }
            if (1 == ctx.children.Count)
            {
                return Visit(ctx.bitExpr(0));
            }
            return VisitRemainPredicate(ctx);
        }

        private PredicateSegment CreateInSegment(MySqlCommandParser.PredicateContext ctx)
        {
            ColumnSegment column = (ColumnSegment)Visit(ctx.bitExpr(0));
            PredicateBracketValue predicateBracketValue = CreateBracketValue(ctx);
            return new PredicateSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, column, new PredicateInRightValue(predicateBracketValue, GetExpressionSegments(ctx)));
        }

        private ICollection<IExpressionSegment> GetExpressionSegments(MySqlCommandParser.PredicateContext ctx)
        {
            ICollection<IExpressionSegment> result = new LinkedList<IExpressionSegment>();
            if (null != ctx.subquery())
            {
                MySqlCommandParser.SubqueryContext subquery = ctx.subquery();
                result.Add(new SubQueryExpressionSegment(new SubQuerySegment(subquery.Start.StartIndex, subquery.Stop.StopIndex, (SelectCommand)Visit(ctx.subquery()))));
                return result;
            }
            foreach (var each in ctx.expr())
            {
                result.Add((IExpressionSegment)Visit(each));
            }
            return result;
        }

        private PredicateBracketValue CreateBracketValue(MySqlCommandParser.PredicateContext ctx)
        {
            PredicateLeftBracketValue predicateLeftBracketValue = null != ctx.subquery()
                    ? new PredicateLeftBracketValue(ctx.subquery().LP_().Symbol.StartIndex, ctx.subquery().LP_().Symbol.StopIndex)
                    : new PredicateLeftBracketValue(ctx.LP_().Symbol.StartIndex, ctx.LP_().Symbol.StopIndex);
            PredicateRightBracketValue predicateRightBracketValue = null != ctx.subquery()
                    ? new PredicateRightBracketValue(ctx.subquery().RP_().Symbol.StartIndex, ctx.subquery().RP_().Symbol.StopIndex)
                    : new PredicateRightBracketValue(ctx.RP_().Symbol.StartIndex, ctx.RP_().Symbol.StopIndex);
            return new PredicateBracketValue(predicateLeftBracketValue, predicateRightBracketValue);
        }

        private PredicateSegment CreateBetweenSegment(MySqlCommandParser.PredicateContext ctx)
        {
            ColumnSegment column = (ColumnSegment)Visit(ctx.bitExpr(0));
            IExpressionSegment between = (IExpressionSegment)Visit(ctx.bitExpr(1));
            IExpressionSegment and = (IExpressionSegment)Visit(ctx.predicate());
            return new PredicateSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, column, new PredicateBetweenRightValue(between, and));
        }

        private IASTNode VisitRemainPredicate(MySqlCommandParser.PredicateContext ctx)
        {
            foreach (var each in ctx.bitExpr())
            {
                Visit(each);
            }
            foreach (var each in ctx.expr())
            {
                Visit(each);
            }
            foreach (var each in ctx.simpleExpr())
            {
                Visit(each);
            }
            if (null != ctx.predicate())
            {
                Visit(ctx.predicate());
            }
            return new CommonExpressionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, ctx.GetText());
        }


        public override IASTNode VisitBitExpr(MySqlCommandParser.BitExprContext ctx)
        {
            if (null != ctx.simpleExpr())
            {
                return CreateExpressionSegment(Visit(ctx.simpleExpr()), ctx);
            }
            VisitRemainBitExpr(ctx);
            return new CommonExpressionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, ctx.GetText());
        }

        private IASTNode CreateExpressionSegment(IASTNode astNode, ParserRuleContext context)
        {
            if (astNode is StringLiteralValue stringLiteralValue)
            {
                return new LiteralExpressionSegment(context.Start.StartIndex, context.Stop.StopIndex, stringLiteralValue.GetValue());
            }
            if (astNode is NumberLiteralValue numberLiteralValue)
            {
                return new LiteralExpressionSegment(context.Start.StartIndex, context.Stop.StopIndex, numberLiteralValue.GetValue());
            }
            if (astNode is BooleanLiteralValue booleanLiteralValue)
            {
                return new LiteralExpressionSegment(context.Start.StartIndex, context.Stop.StopIndex, booleanLiteralValue.GetValue());
            }
            if (astNode is ParameterMarkerValue parameterMarkerValue)
            {
                return new ParameterMarkerExpressionSegment(context.Start.StartIndex, context.Stop.StopIndex, parameterMarkerValue.GetValue(), context.GetText());
            }
            if (astNode is SubQuerySegment subQuerySegment)
            {
                return new SubQueryExpressionSegment(subQuerySegment);
            }
            if (astNode is OtherLiteralValue)
            {
                return new CommonExpressionSegment(context.Start.StartIndex, context.Stop.StopIndex, context.GetText());
            }
            return astNode;
        }

        private void VisitRemainBitExpr(MySqlCommandParser.BitExprContext ctx)
        {
            if (null != ctx.intervalExpression())
            {
                Visit(ctx.intervalExpression().expr());
            }

            foreach (var bitExprContext in ctx.bitExpr())
            {
                Visit(bitExprContext);
            }
        }


        public override IASTNode VisitSimpleExpr(MySqlCommandParser.SimpleExprContext ctx)
        {
            if (null != ctx.subquery())
            {
                return new SubQuerySegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, (SelectCommand)Visit(ctx.subquery()));
            }
            if (null != ctx.parameterMarker())
            {
                return Visit(ctx.parameterMarker());
            }
            if (null != ctx.literals())
            {
                return Visit(ctx.literals());
            }
            if (null != ctx.intervalExpression())
            {
                return Visit(ctx.intervalExpression());
            }
            if (null != ctx.functionCall())
            {
                return Visit(ctx.functionCall());
            }
            if (null != ctx.columnName())
            {
                return Visit(ctx.columnName());
            }
            return VisitRemainSimpleExpr(ctx);
        }


        public override IASTNode VisitIntervalExpression(MySqlCommandParser.IntervalExpressionContext ctx)
        {
            CalculateParameterCount(new List<MySqlCommandParser.ExprContext>() { ctx.expr() });
            return new ExpressionProjectionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, ctx.GetText());
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
            throw new ShardingException("FunctionCallContext must have aggregationFunction, regularFunction or specialFunction.");
        }


        public override IASTNode VisitAggregationFunction(MySqlCommandParser.AggregationFunctionContext ctx)
        {
            String aggregationType = ctx.aggregationFunctionName().GetText();
            return AggregationType.IsAggregationType(aggregationType)
                    ? CreateAggregationSegment(ctx, aggregationType) : new ExpressionProjectionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, ctx.GetText());
        }

        private IASTNode CreateAggregationSegment(MySqlCommandParser.AggregationFunctionContext ctx, String aggregationType)
        {
            AggregationTypeEnum type = AggregationType.ValueOf(aggregationType.ToUpper());
            int innerExpressionStartIndex = ((ITerminalNode)ctx.GetChild(1)).Symbol.StartIndex;
            if (null == ctx.distinct())
            {
                return new AggregationProjectionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, type, innerExpressionStartIndex);
            }
            return new AggregationDistinctProjectionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, type, innerExpressionStartIndex, getDistinctExpression(ctx));
        }

        private String getDistinctExpression(MySqlCommandParser.AggregationFunctionContext ctx)
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
            if (null != ctx.weightStringFunction())
            {
                return Visit(ctx.weightStringFunction());
            }
            return new ExpressionProjectionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, ctx.GetText());
        }


        public override IASTNode VisitGroupConcatFunction(MySqlCommandParser.GroupConcatFunctionContext ctx)
        {
            CalculateParameterCount(ctx.expr());
            return new ExpressionProjectionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, ctx.GetText());
        }


        public override IASTNode VisitWindowFunction(MySqlCommandParser.WindowFunctionContext ctx)
        {
            CalculateParameterCount(ctx.expr());
            return new ExpressionProjectionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, ctx.GetText());
        }


        public override IASTNode VisitCastFunction(MySqlCommandParser.CastFunctionContext ctx)
        {
            CalculateParameterCount(new List<MySqlCommandParser.ExprContext>() { ctx.expr() });
            return new ExpressionProjectionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, ctx.GetText());
        }


        public override IASTNode VisitConvertFunction(MySqlCommandParser.ConvertFunctionContext ctx)
        {
            CalculateParameterCount(new List<MySqlCommandParser.ExprContext>() { ctx.expr() });
            return new ExpressionProjectionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, ctx.GetText());
        }


        public override IASTNode VisitPositionFunction(MySqlCommandParser.PositionFunctionContext ctx)
        {
            CalculateParameterCount(ctx.expr());
            return new ExpressionProjectionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, ctx.GetText());
        }


        public override IASTNode VisitSubstringFunction(MySqlCommandParser.SubstringFunctionContext ctx)
        {
            CalculateParameterCount(new List<MySqlCommandParser.ExprContext>() { ctx.expr() });
            return new ExpressionProjectionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, ctx.GetText());
        }


        public override IASTNode VisitExtractFunction(MySqlCommandParser.ExtractFunctionContext ctx)
        {
            CalculateParameterCount(new List<MySqlCommandParser.ExprContext>() { ctx.expr() });
            return new ExpressionProjectionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, ctx.GetText());
        }


        public override IASTNode VisitCharFunction(MySqlCommandParser.CharFunctionContext ctx)
        {
            CalculateParameterCount(ctx.expr());
            return new ExpressionProjectionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, ctx.GetText());
        }


        public override IASTNode VisitWeightStringFunction(MySqlCommandParser.WeightStringFunctionContext ctx)
        {
            CalculateParameterCount(new List<MySqlCommandParser.ExprContext>() { ctx.expr() });
            return new ExpressionProjectionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, ctx.GetText());
        }


        public override IASTNode VisitRegularFunction(MySqlCommandParser.RegularFunctionContext ctx)
        {
            CalculateParameterCount(ctx.expr());
            return new ExpressionProjectionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, ctx.GetText());
        }

        private IASTNode VisitRemainSimpleExpr(MySqlCommandParser.SimpleExprContext ctx)
        {
            if (null != ctx.caseExpression())
            {
                return Visit(ctx.caseExpression());
            }
            foreach (var each in ctx.expr())
            {
                Visit(each);
            }
            foreach (var each in ctx.simpleExpr())
            {
                Visit(each);
            }
            return new CommonExpressionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, ctx.GetText());
        }


        public override IASTNode VisitCaseExpression(MySqlCommandParser.CaseExpressionContext ctx)
        {
            return new OtherLiteralValue(ctx.GetText());
        }


        public override IASTNode VisitDataTypeName(MySqlCommandParser.DataTypeNameContext ctx)
        {
            return new KeywordValue(ctx.GetText());
        }

        // TODO :FIXME, sql case id: insert_with_str_to_date
        private void CalculateParameterCount(ICollection<MySqlCommandParser.ExprContext> exprContexts)
        {
            foreach (var each in exprContexts)
            {
                Visit(each);
            }
        }


        public override IASTNode VisitDataType(MySqlCommandParser.DataTypeContext ctx)
        {
            DataTypeSegment dataTypeSegment = new DataTypeSegment();
            dataTypeSegment.SetDataTypeName(((KeywordValue)Visit(ctx.dataTypeName())).GetValue());
            dataTypeSegment.SetStartIndex(ctx.Start.StartIndex);
            dataTypeSegment.SetStopIndex(ctx.Stop.StopIndex);
            if (null != ctx.dataTypeLength())
            {
                DataTypeLengthSegment dataTypeLengthSegment = (DataTypeLengthSegment)Visit(ctx.dataTypeLength());
                dataTypeSegment.SetDataLength(dataTypeLengthSegment);
            }
            return dataTypeSegment;
        }


        public override IASTNode VisitDataTypeLength(MySqlCommandParser.DataTypeLengthContext ctx)
        {
            DataTypeLengthSegment dataTypeLengthSegment = new DataTypeLengthSegment();
            dataTypeLengthSegment.SetStartIndex(ctx.Start.StartIndex);
            dataTypeLengthSegment.SetStopIndex(ctx.Stop.StartIndex);
            ITerminalNode[] numbers = ctx.NUMBER_();
            if (numbers.Length == 1)
            {
                dataTypeLengthSegment.SetPrecision(int.Parse(numbers[0].GetText()));
            }
            if (numbers.Length == 2)
            {
                dataTypeLengthSegment.SetPrecision(int.Parse(numbers[0].GetText()));
                dataTypeLengthSegment.SetScale(int.Parse(numbers[1].GetText()));
            }
            return dataTypeLengthSegment;
        }


        public override IASTNode VisitOrderByClause(MySqlCommandParser.OrderByClauseContext ctx)
        {
            ICollection<OrderByItemSegment> items = new LinkedList<OrderByItemSegment>();
            foreach (var each in ctx.orderByItem())
            {
                items.Add((OrderByItemSegment)Visit(each));
            }
            return new OrderBySegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, items);
        }


        public override IASTNode VisitOrderByItem(MySqlCommandParser.OrderByItemContext ctx)
        {
            OrderDirectionEnum orderDirection = null != ctx.DESC() ? OrderDirectionEnum.DESC : OrderDirectionEnum.ASC;
            if (null != ctx.columnName())
            {
                ColumnSegment column = (ColumnSegment)Visit(ctx.columnName());
                return new ColumnOrderByItemSegment(column, orderDirection);
            }
            if (null != ctx.numberLiterals())
            {
                return new IndexOrderByItemSegment(ctx.numberLiterals().Start.StartIndex, ctx.numberLiterals().Stop.StopIndex,
                        (int)SqlUtil.GetExactlyNumber(ctx.numberLiterals().GetText(), 10), orderDirection);
            }
            return new ExpressionOrderByItemSegment(ctx.expr().Start.StartIndex, ctx.expr().Stop.StopIndex, ctx.expr().GetText(), orderDirection);
        }
    }
}
