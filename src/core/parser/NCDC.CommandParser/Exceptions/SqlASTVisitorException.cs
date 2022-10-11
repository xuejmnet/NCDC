using System.Runtime.Serialization;
using Antlr4.Runtime.Tree;

namespace NCDC.CommandParser.Exceptions;

public class SqlASTVisitorException:SqlParsingBaseException
{

    public SqlASTVisitorException(Type type) : base($"Can not accept SQL type {type.FullName}")
    {
    }
}