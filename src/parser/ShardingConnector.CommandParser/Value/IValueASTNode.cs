using ShardingConnector.AbstractParser;

namespace ShardingConnector.CommandParser.Value
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/10 10:09:53
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public interface IValueASTNode<out T>:IASTNode
    {
        T GetValue();
    }
}
