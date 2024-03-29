﻿using NCDC.CommandParser.Common.Constant;
using NCDC.CommandParser.Common.Util;

namespace NCDC.CommandParser.Common.Value.Identifier
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

        public string Value { get; }
        public  QuoteCharacterEnum QuoteCharacter { get; }

        public IdentifierValue(string value,QuoteCharacterEnum quoteCharacter)
        {
            Value = value;
            QuoteCharacter = quoteCharacter;
        }
        public IdentifierValue(string text):this(SqlUtil.GetExactlyValue(text)!,NCDC.CommandParser.Common.Constant.QuoteCharacter.GetQuoteCharacter(text))
        {
        }
        public IdentifierValue(string text,string reservedCharacters):this(SqlUtil.GetExactlyValue(text,reservedCharacters)!,NCDC.CommandParser.Common.Constant.QuoteCharacter.GetQuoteCharacter(text))
        {
        }


        public override string ToString()
        {
            return $"{nameof(Value)}: {Value}, {nameof(QuoteCharacter)}: {QuoteCharacter}";
        }

    }
}
