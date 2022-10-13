using System.Collections.Generic;
using System.Linq;

namespace NCDC.CommandParser.Common.Constant
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 8:19:00
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    /// <summary>
    /// 括号
    /// </summary>
    public class Paren
    {
        private static readonly IDictionary<ParenEnum, Paren> _parens = new Dictionary<ParenEnum, Paren>();
        /// <summary>
        /// 左括号
        /// </summary>
        private readonly char _leftParen;
        /// <summary>
        /// 右括号
        /// </summary>
        private readonly char _rightParen;

        static Paren()
        {
            _parens.Add(ParenEnum.PARENTHESES, new Paren('(', ')'));
            _parens.Add(ParenEnum.BRACKETS, new Paren('[', ']'));
            _parens.Add(ParenEnum.BRACES, new Paren('{', '}'));
        }
        private Paren(char leftParen, char rightParen)
        {
            _leftParen = leftParen;
            _rightParen = rightParen;
        }
        /// <summary>
        /// 是否存在左括号
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool IsLeftParen(char token)
        {
            return _parens.Any(p => p.Value._leftParen == token);
        }
        /// <summary>
        /// 是否匹配左右括号
        /// </summary>
        /// <param name="leftToken"></param>
        /// <param name="rightToken"></param>
        /// <returns></returns>
        public static bool Match(char leftToken, char rightToken)
        {
            return _parens.Any(p => p.Value._leftParen == leftToken&&p.Value._rightParen==rightToken);
        }

        public char GetLeftParen()
        {
            return _leftParen;
        }
        public char GetRightParen()
        {
            return _rightParen;
        }

        public static Paren Get(ParenEnum paren)
        {
            return _parens[paren];
        }
    }

    public enum ParenEnum
    {
        /// <summary>
        /// 圆括号
        /// </summary>
        PARENTHESES=1,
        /// <summary>
        /// 中括号
        /// </summary>
        BRACKETS=1<<1,
        /// <summary>
        /// 大括号
        /// </summary>
        BRACES=1<<2
    }
}
