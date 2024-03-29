﻿using System.Text.RegularExpressions;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Common.Constant;
using NCDC.CommandParser.Common.Segment.DML.Expr;
using NCDC.CommandParser.Common.Segment.DML.Expr.Complex;
using NCDC.CommandParser.Common.Segment.DML.Expr.Simple;
using NCDC.CommandParser.Common.Segment.Generic.Table;
using NCDC.CommandParser.Common.Value.Literal.Impl;
using NCDC.Extensions;

namespace NCDC.CommandParser.Common.Util
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/10 13:04:39
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public static class SqlUtil
    {
        private const string SQL_END=";";
        private const string COMMENT_PREFIX = "/*";
        private const string COMMENT_SUFFIX = "*/";
        private const string EXCLUDED_CHARACTERS = "[]`'\"";
        private static readonly Regex SINGLE_CHARACTER_PATTERN = new Regex("^_|([^\\\\])_");
        private static readonly Regex SINGLE_CHARACTER_ESCAPE_PATTERN = new Regex("\\\\_");
        private static readonly Regex ANY_CHARACTER_PATTERN = new Regex("^%|([^\\\\])%");
        private static readonly Regex ANY_CHARACTER_ESCAPE_PATTERN = new Regex("\\\\%");
        
        

        public static decimal GetExactlyNumber(string value, int radix)
        {
            try
            {
                return Convert.ToInt64(value, radix);
            }
            catch (FormatException e)
            {
                return decimal.Parse(value);
            }
        }

        /// <summary>
        /// remove special char for SQL expression
        /// 获取移除了特殊字符后的值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string? GetExactlyValue(string? value)
        {
            if (value == null)
                return null;
            return value.Replace("[",string.Empty)
                .Replace("]", string.Empty)
                .Replace("`", string.Empty)
                .Replace("'", string.Empty)
                .Replace("\"", string.Empty);
        }
        public static string? GetExactlyValue(string? value,string reservedCharacters)
        {
            if (value == null)
                return null;
            var charArray = EXCLUDED_CHARACTERS.ToCharArray();
            charArray.RemoveAll(reservedCharacters.ToCharArray());
            var valueChars = value.ToCharArray();
            valueChars.RemoveAll(charArray);
            return new string(valueChars);
        }

        /// <summary>
        /// 移除空格
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetExactlyExpression(string value)
        {
            return value?.Replace(" ",string.Empty);
        }
        /// <summary>
        /// 获取不带外圆括号的表达式
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetExpressionWithoutOutsideParentheses(string value)
        {
            int parenthesesOffset = GetParenthesesOffset(value);
            return 0 == parenthesesOffset ? value : value.SubStringWithEndIndex(parenthesesOffset, value.Length - parenthesesOffset);
        }
        private static int GetParenthesesOffset(string value)
        {
            int result = 0;
            if (string.IsNullOrEmpty(value)) {
                return result;
            }
            while (Paren.Get(ParenEnum.PARENTHESES).GetLeftParen() == value[result])
            {
                result++;
            }
            return result;
        }


        /// <summary>
        /// 去掉注解
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string TrimComment(string sql)
        {
            var result = sql;
            if (sql.StartsWith(COMMENT_PREFIX))
            {
                result = result.Substring(sql.IndexOf(COMMENT_PREFIX, StringComparison.Ordinal) + 2);
            }

            if (sql.EndsWith(SQL_END))
            {
                result = result.Substring(0, result.Length - 1);
            }

            return result.Trim();
        }
        public static IExpressionSegment CreateLiteralExpression(IASTNode astNode,  int startIndex,  int stopIndex,  String text) {
            if (astNode is StringLiteralValue stringLiteralValue) {
                return new LiteralExpressionSegment(startIndex, stopIndex, stringLiteralValue.Value);
            }
            if (astNode is NumberLiteralValue numberLiteralValue) {
                return new LiteralExpressionSegment(startIndex, stopIndex, numberLiteralValue.Value);
            }
            if (astNode is BooleanLiteralValue booleanLiteralValue) {
                return new LiteralExpressionSegment(startIndex, stopIndex,booleanLiteralValue.Value);
            }
            if (astNode is OtherLiteralValue otherLiteralValue) {
                return new CommonExpressionSegment(startIndex, stopIndex, otherLiteralValue.Value);
            }
            return new CommonExpressionSegment(startIndex, stopIndex, text);
        }
        public static List<SubQueryTableSegment> GetSubQueryTableSegmentFromTableSegment(ITableSegment tableSegment) {
            List<SubQueryTableSegment> result = new List<SubQueryTableSegment>();
            if (tableSegment is SubQueryTableSegment subQueryTableSegment) {
                result.Add(subQueryTableSegment);
            }
            if (tableSegment is JoinTableSegment joinTableSegment) {
                result.AddAll(GetSubqueryTableSegmentFromJoinTableSegment(joinTableSegment));
            }
            return result;
        }
    
        private static List<SubQueryTableSegment> GetSubqueryTableSegmentFromJoinTableSegment( JoinTableSegment joinTableSegment) {
            List<SubQueryTableSegment> result = new List<SubQueryTableSegment>();
            if (joinTableSegment.Left is SubQueryTableSegment subQueryTableSegmentLeft) {
                result.Add(subQueryTableSegmentLeft);
            } else if (joinTableSegment.Left is JoinTableSegment joinTableSegmentLeft) {
                result.AddAll(GetSubqueryTableSegmentFromJoinTableSegment(joinTableSegmentLeft));
            }
            if (joinTableSegment.Right is SubQueryTableSegment subQueryTableSegmentRight) {
                result.Add(subQueryTableSegmentRight);
            } else if (joinTableSegment.Right is JoinTableSegment joinTableSegmentRight) {
                result.AddAll(GetSubqueryTableSegmentFromJoinTableSegment(joinTableSegmentRight));
            }
            return result;
        }
    }
}
