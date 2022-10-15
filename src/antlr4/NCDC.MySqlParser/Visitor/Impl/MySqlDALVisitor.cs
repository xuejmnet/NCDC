using System;
using System.Collections.Generic;
using System.Text;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Abstractions.Visitor.Commands;
using NCDC.CommandParser.Common.Segment.DAL;
using NCDC.CommandParser.Common.Segment.Generic;
using NCDC.CommandParser.Common.Segment.Generic.Table;
using NCDC.CommandParser.Common.Value.Identifier;
using NCDC.CommandParser.Dialect.Command.MySql.DAL;
using NCDC.Extensions;


namespace NCDC.MySqlParser.Visitor.Impl
{
    /// <summary>
    /// 
    /// </summary>
    /// Author: xjm
    /// Created: 2022/5/10 9:05:14
    /// Email: 326308290@qq.com
    public sealed class MySqlDALVisitor : MySqlVisitor, IDALVisitor
    {
        public override IASTNode VisitUse(MySqlCommandParser.UseContext ctx)
        {
            return new MySqlUseCommand(((IdentifierValue)Visit(ctx.schemaName())).Value);
        }


        public override IASTNode VisitFromTable(MySqlCommandParser.FromTableContext ctx)
        {
            FromTableSegment result = new FromTableSegment();
            result.Table = (SimpleTableSegment)Visit(ctx.tableName());
            return result;
        }

        public override IASTNode VisitSetVariable(MySqlCommandParser.SetVariableContext ctx)
        {
            var result = new MySqlSetCommand();
            ICollection<VariableAssignSegment> variableAssigns = GetVariableAssigns(ctx.optionValueList());
            result.VariableAssigns.ToList().AddAll(variableAssigns);
            return result;
        }

        private ICollection<VariableAssignSegment> GetVariableAssigns(MySqlCommandParser.OptionValueListContext ctx)
        {
            ICollection<VariableAssignSegment> result = new LinkedList<VariableAssignSegment>();
            if (null == ctx.optionValueNoOptionType())
            {
                VariableAssignSegment variableAssign = new VariableAssignSegment();
                variableAssign.StartIndex = ctx.Start.StartIndex;
                variableAssign.StopIndex = ctx.setExprOrDefault().Stop.StopIndex;
                VariableSegment variable = new VariableSegment();
                variable.Scope = ctx.optionType().GetText();
                variable.Variable = ctx.internalVariableName().GetText();
                variableAssign.Variable = variable;
                variableAssign.AssignValue = ctx.setExprOrDefault().GetText();
                result.Add(variableAssign);
            }
            else
            {
                result.Add(GetVariableAssign(ctx.optionValueNoOptionType()));
            }

            foreach (var optionValueContext in ctx.optionValue())
            {
                result.Add(GetVariableAssign(optionValueContext));
            }

            return result;
        }

        private VariableAssignSegment GetVariableAssign(MySqlCommandParser.OptionValueNoOptionTypeContext ctx)
        {
            VariableAssignSegment result = new VariableAssignSegment();
            result.StartIndex = ctx.Start.StartIndex;
            result.StopIndex = ctx.Stop.StopIndex;
            VariableSegment variable = new VariableSegment();
            if (null != ctx.NAMES())
            {
                variable.Variable = "charset";
                result.Variable = variable;
                result.AssignValue = ctx.charsetName().GetText();
            }
            else if (null != ctx.internalVariableName())
            {
                variable.Variable = ctx.internalVariableName().GetText();
                result.Variable = variable;
                result.AssignValue = ctx.setExprOrDefault().GetText();
            }
            else if (null != ctx.userVariable())
            {
                variable.Variable = ctx.userVariable().GetText();
                result.Variable = variable;
                result.AssignValue = ctx.expr().GetText();
            }
            else if (null != ctx.setSystemVariable())
            {
                variable.Variable = ctx.setSystemVariable().internalVariableName().GetText();
                result.Variable = variable;
                result.AssignValue = ctx.setExprOrDefault().GetText();
                MySqlCommandParser.OptionTypeContext optionType = ctx.setSystemVariable().optionType();
                variable.Scope = optionType?.GetText() ?? "SESSION";
            }

            return result;
        }

        private VariableAssignSegment GetVariableAssign(MySqlCommandParser.OptionValueContext ctx)
        {
            VariableAssignSegment result = new VariableAssignSegment();
            result.StartIndex = ctx.Start.StartIndex;
            result.StopIndex = ctx.Stop.StopIndex;
            VariableSegment variable = new VariableSegment();
            if (null != ctx.optionValueNoOptionType())
            {
                return GetVariableAssign(ctx.optionValueNoOptionType());
            }

            variable.Scope = ctx.optionType().GetText();
            variable.Variable = ctx.internalVariableName().GetText();
            result.Variable = variable;
            result.AssignValue = ctx.setExprOrDefault().GetText();
            return result;
        }

        public override IASTNode VisitSetCharacter(MySqlCommandParser.SetCharacterContext ctx)
        {
            VariableAssignSegment characterSet = new VariableAssignSegment();
            VariableSegment variable = new VariableSegment();
            String variableName = (null != ctx.CHARSET()) ? ctx.CHARSET().GetText() : "charset";
            variable.Variable=variableName;
            characterSet.Variable=variable;
            String assignValue = (null != ctx.DEFAULT()) ? ctx.DEFAULT().GetText() : ctx.charsetName().GetText();
            characterSet.AssignValue=assignValue;
            MySqlSetCommand result = new MySqlSetCommand();
            result.VariableAssigns.Add(characterSet);
            return result;
        }


        public override IASTNode VisitFromSchema(MySqlCommandParser.FromSchemaContext ctx)
        {
            return new FromSchemaSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex,(DatabaseSegment)Visit(ctx.schemaName()));
        }
    }
}