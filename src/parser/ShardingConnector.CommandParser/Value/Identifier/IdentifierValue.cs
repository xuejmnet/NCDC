using ShardingConnector.CommandParser.Constant;
using ShardingConnector.CommandParser.Util;

namespace ShardingConnector.CommandParser.Value.Identifier
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/10 10:09:27
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class IdentifierValue : IValueASTNode<string>
    {
        private readonly string _value;

        private readonly QuoteCharacterEnum _quoteCharacterEnum;

        public IdentifierValue(string text)
        {
            _value = SqlUtil.GetExactlyValue(text);
            _quoteCharacterEnum = QuoteCharacter.GetQuoteCharacter(text);
        }
        public string GetValue()
        {
            return _value;
        }

        public QuoteCharacterEnum GetQuoteCharacter()
        {
            return _quoteCharacterEnum;
        }
    }
}
