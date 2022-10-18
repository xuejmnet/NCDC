using NCDC.CommandParser.Common.Command;
using NCDC.CommandParser.Common.Command.DDL;
using NCDC.CommandParser.Common.Command.DML;

namespace NCDC.ProxyServer.Helpers;

public class AutoCommitHelper
{
    public static bool NeedOpenTransaction(ISqlCommand sqlCommand)
    {
        if (sqlCommand is SelectCommand selectCommand && null == selectCommand.From)
        {
            return false;
        }

        if (sqlCommand is IDDLCommand || sqlCommand is IDMLCommand)
        {
            return true;
        }

        return false;
    }
}