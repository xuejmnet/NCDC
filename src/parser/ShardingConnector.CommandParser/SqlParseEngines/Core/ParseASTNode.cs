using Antlr4.Runtime.Tree;
using ShardingConnector.Parsers;

namespace ShardingConnector.ParserEngine.Core
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

        public ParseASTNode(IParseTree parseTree)
        {
            _parseTree = parseTree;
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
    }
}
