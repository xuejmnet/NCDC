using OpenConnector.Parsers.Visitor.Commands;

namespace OpenConnector.Parsers.Visitor
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 8:42:03
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    /// <summary>
    /// 外观模式
    /// </summary>
    public interface ISqlVisitorCreator
    {
        IDMLVisitor CreateDMLVisitor();
        IDDLVisitor CreateDDLVisitor();
        ITCLVisitor CreateTCLVisitor();
        IDCLVisitor CreateDCLVisitor();
        IDALVisitor CreateDALVisitor();
        IRLVisitor CreateRLVisitor();
    }
}
