using MySqlConnector;
using NCDC.Protocol.MySql.Constant;

namespace NCDC.ProxyClientMySql.Extensions;

public static class MySqlExtension
{
    public static MySqlColumnTypeEnum GetMySqlColumnType(this MySqlDbColumn dbColumn,MySqlCharacterSet characterSet,int columnSize,int columnFlags)
    {
        switch (dbColumn.ProviderType)
        {
            case MySqlDbType.Bool:
            case MySqlDbType.UByte:
            case MySqlDbType.Byte:
                return MySqlColumnTypeEnum.Tiny;
            case MySqlDbType.UInt24:
            case MySqlDbType.Int24:
                return MySqlColumnTypeEnum.Int24;
            case MySqlDbType.UInt32:
            case MySqlDbType.Int32:
                return MySqlColumnTypeEnum.Long;
            case MySqlDbType.Int64:
            case MySqlDbType.UInt64:
                return MySqlColumnTypeEnum.Longlong;
            case MySqlDbType.Bit:
                return MySqlColumnTypeEnum.Bit;
            case MySqlDbType.Guid:
            case MySqlDbType.Binary:
            case MySqlDbType.String:
                return MySqlColumnTypeEnum.String;

            case MySqlDbType.Enum:
            {
                var flags = (MySqlColumnFlagsEnum)columnFlags;
                if (flags.HasFlag(MySqlColumnFlagsEnum.Enum))
                {
                    return MySqlColumnTypeEnum.Enum;
                }
                return MySqlColumnTypeEnum.String;
            }
            case MySqlDbType.Set:
            {
                var flags = (MySqlColumnFlagsEnum)columnFlags;
                if (flags.HasFlag(MySqlColumnFlagsEnum.Set))
                {
                    return MySqlColumnTypeEnum.Set;
                }
                return MySqlColumnTypeEnum.String;
            }
            case MySqlDbType.VarBinary:
            case MySqlDbType.VarChar:
                return MySqlColumnTypeEnum.VarString;
            case MySqlDbType.TinyBlob:
                return MySqlColumnTypeEnum.TinyBlob;
            case MySqlDbType.Blob:
                return MySqlColumnTypeEnum.Blob;
            case MySqlDbType.MediumBlob:
                return MySqlColumnTypeEnum.MediumBlob;
            case MySqlDbType.LongBlob:
                return MySqlColumnTypeEnum.LongBlob;
            case MySqlDbType.TinyText:
                return MySqlColumnTypeEnum.TinyBlob;
            case MySqlDbType.Text:
                return MySqlColumnTypeEnum.Blob;
            case MySqlDbType.MediumText:
                return MySqlColumnTypeEnum.MediumBlob;
            case MySqlDbType.LongText:
                return MySqlColumnTypeEnum.LongBlob;
            case MySqlDbType.JSON:
                return MySqlColumnTypeEnum.Json;
            case MySqlDbType.UInt16:
            case MySqlDbType.Int16:
                return MySqlColumnTypeEnum.Short;
            case MySqlDbType.Date:
                return MySqlColumnTypeEnum.NewDate;//NewDate


            case MySqlDbType.DateTime:
                return MySqlColumnTypeEnum.DateTime;

            case MySqlDbType.Timestamp:
                return MySqlColumnTypeEnum.Timestamp;
            case MySqlDbType.Time:
                return MySqlColumnTypeEnum.Time;
            case MySqlDbType.Year:
                return MySqlColumnTypeEnum.Year;

            case MySqlDbType.Float:
                return MySqlColumnTypeEnum.Float;

            case MySqlDbType.Double:
                return MySqlColumnTypeEnum.Double;

            case MySqlDbType.Decimal:
                return MySqlColumnTypeEnum.Decimal;
            case MySqlDbType.NewDecimal:
                return MySqlColumnTypeEnum.NewDecimal;
            case MySqlDbType.Geometry:
                return MySqlColumnTypeEnum.Geometry;
            case MySqlDbType.Null:
                return MySqlColumnTypeEnum.Null;


            default:
                throw new NotImplementedException($"ConvertToMySqlDbType for {dbColumn.ProviderType} is not implemented");
           
        }

    }
}