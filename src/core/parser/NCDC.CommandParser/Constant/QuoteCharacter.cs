using System.Collections.Generic;

namespace NCDC.CommandParser.Constant
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/10 10:11:53
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public class QuoteCharacter
    {
        private static readonly IDictionary<QuoteCharacterEnum, QuoteCharacter> _quotes = new Dictionary<QuoteCharacterEnum, QuoteCharacter>();
        private readonly string _startDelimiter;
        private readonly string _endDelimiter;

        static QuoteCharacter()
        {
            _quotes.Add(QuoteCharacterEnum.BACK_QUOTE, new QuoteCharacter("`", "`"));
            _quotes.Add(QuoteCharacterEnum.SINGLE_QUOTE, new QuoteCharacter("'", "'"));
            _quotes.Add(QuoteCharacterEnum.QUOTE, new QuoteCharacter("\"", "\""));
            _quotes.Add(QuoteCharacterEnum.BRACKETS, new QuoteCharacter("[", "]"));
            _quotes.Add(QuoteCharacterEnum.PARENTHESES, new QuoteCharacter("(", ")"));
            _quotes.Add(QuoteCharacterEnum.NONE, new QuoteCharacter("", ""));
        }
        private QuoteCharacter(string startDelimiter, string endDelimiter)
        {
            _startDelimiter = startDelimiter;
            _endDelimiter = endDelimiter;
        }

        public static QuoteCharacterEnum GetQuoteCharacter(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return QuoteCharacterEnum.NONE;
            foreach (var quote in _quotes)
            {
                if (QuoteCharacterEnum.NONE != quote.Key && quote.Value._startDelimiter[0] == value[0])
                    return quote.Key;
            }
            return QuoteCharacterEnum.NONE;
        }

        public string GetStartDelimiter()
        {
            return _startDelimiter;
        }

        public string GetEndDelimiter()
        {
            return _endDelimiter;
        }

        public static QuoteCharacter Get(QuoteCharacterEnum quoteCharacterEnum)
        {
            return _quotes[quoteCharacterEnum];
        }
    }

    public enum QuoteCharacterEnum
    {
        BACK_QUOTE,
        SINGLE_QUOTE,
        QUOTE,
        BRACKETS,
        PARENTHESES,
        NONE
    }
}
