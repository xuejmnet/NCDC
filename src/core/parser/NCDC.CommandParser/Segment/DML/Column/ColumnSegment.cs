using System.Text;
using NCDC.CommandParser.Segment.DML.Expr;
using NCDC.CommandParser.Segment.DML.Predicate.Value;
using NCDC.CommandParser.Segment.Generic;
using NCDC.CommandParser.Value.Identifier;

namespace NCDC.CommandParser.Segment.DML.Column
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/10 15:17:49
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ColumnSegment:IExpressionSegment,IOwnerAvailable
    {
        public int StartIndex { get; }
        public int StopIndex { get; }
        public IdentifierValue IdentifierValue { get; }

        public OwnerSegment? Owner { get; set; }
        private string? OwnerValue => Owner?.IdentifierValue.Value;

        public ColumnSegment(int startIndex, int stopIndex, IdentifierValue identifierValue)
        {
            StartIndex = startIndex;
            StopIndex = stopIndex;
            IdentifierValue = identifierValue;
        }

        /// <summary>
        /// 获取所属值如table.column
        /// </summary>
        /// <returns></returns>
        public string GetQualifiedName() {
            return null == Owner ? IdentifierValue.Value : Owner.IdentifierValue.Value + "." + IdentifierValue.Value;
        }

        public string GetExpression()
        {
            return  null == Owner ? IdentifierValue.Value :$"{Owner.IdentifierValue.Value}.{IdentifierValue.Value}";
        }

        public override int GetHashCode()
        {
            return $"{OwnerValue}{IdentifierValue.Value}".GetHashCode();
        }
    }
}
