using NCDC.CommandParser.Abstractions.Visitor.Commands;

namespace NCDC.CommandParser.Abstractions.Visitor
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
