using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using NCDC.CommandParser.Abstractions;

namespace NCDC.CommandParser.Core
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/7 8:30:46
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ParseASTNode:IASTNode
    {
        private readonly IParseTree _parseTree;
        private readonly CommonTokenStream _commonTokenStream;

        public ParseASTNode(IParseTree parseTree,CommonTokenStream commonTokenStream)
        {
            _parseTree = parseTree;
            _commonTokenStream = commonTokenStream;
        }
    
        /**
     * Get root node.
     * 
     * @return root node
     */
        public IParseTree GetRootNode()
        {
            return _parseTree.GetChild(0);
        }

        public IEnumerable<IToken> GetHiddenTokens()
        {
            foreach (var token in _commonTokenStream.GetTokens())
            {
                if (TokenConstants.HiddenChannel == token.Channel)
                {
                    yield return token;
                }
            }
        }
    }
}
