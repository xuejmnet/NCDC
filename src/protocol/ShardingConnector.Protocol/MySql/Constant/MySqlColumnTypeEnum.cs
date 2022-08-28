using System.Collections.Immutable;

namespace ShardingConnector.Protocol.MySql.Constant;

public enum MySqlColumnTypeEnum
{
    MYSQL_TYPE_DECIMAL=0x00,
    
    MYSQL_TYPE_TINY=0x01,
    
    MYSQL_TYPE_SHORT=0x02,
    
    MYSQL_TYPE_LONG=0x03,
    
    MYSQL_TYPE_FLOAT=0x04,
    
    MYSQL_TYPE_DOUBLE=0x05,
    
    MYSQL_TYPE_NULL=0x06,
    
    MYSQL_TYPE_TIMESTAMP=0x07,
    
    MYSQL_TYPE_LONGLONG=0x08,
    
    MYSQL_TYPE_INT24=0x09,
    
    MYSQL_TYPE_DATE=0x0a,
    
    MYSQL_TYPE_TIME=0x0b,
    
    MYSQL_TYPE_DATETIME=0x0c,
    
    MYSQL_TYPE_YEAR=0x0d,
    
    MYSQL_TYPE_NEWDATE=0x0e,
    
    MYSQL_TYPE_VARCHAR=0x0f,
    
    MYSQL_TYPE_BIT=0x10,
    
    MYSQL_TYPE_TIMESTAMP2=0x11,
    
    MYSQL_TYPE_DATETIME2=0x12,
    
    MYSQL_TYPE_TIME2=0x13,
    MySQL_TYPE_JSON=0xf5,
    
    MYSQL_TYPE_NEWDECIMAL=0xf6,
    
    MYSQL_TYPE_ENUM=0xf7,
    
    MYSQL_TYPE_SET=0xf8,
    
    MYSQL_TYPE_TINY_BLOB=0xf9,
    
    MYSQL_TYPE_MEDIUM_BLOB=0xfa,
    
    MYSQL_TYPE_LONG_BLOB=0xfb,
    
    MYSQL_TYPE_BLOB=0xfc,
    
    MYSQL_TYPE_VAR_STRING=0xfd,
    
    MYSQL_TYPE_STRING=0xfe,
    
    MYSQL_TYPE_GEOMETRY=0xff
}

public sealed class MySqlColumnTypeMapping
{
    private static readonly ImmutableDictionary<Type, MySqlColumnTypeEnum> CLR_TYPE_TO_MYSQL_COLUMN_TYPES;

    static MySqlColumnTypeMapping()
    {
        var mySqlColumnTypeEnums = new Dictionary<Type, MySqlColumnTypeEnum>();
         mySqlColumnTypeEnums.Add(typeof(bool),MySqlColumnTypeEnum.MYSQL_TYPE_BIT);
         mySqlColumnTypeEnums.Add(typeof(byte),MySqlColumnTypeEnum.MYSQL_TYPE_TINY);
         mySqlColumnTypeEnums.Add(typeof(short),MySqlColumnTypeEnum.MYSQL_TYPE_SHORT);
         mySqlColumnTypeEnums.Add(typeof(int),MySqlColumnTypeEnum.MYSQL_TYPE_LONG);
         mySqlColumnTypeEnums.Add(typeof(long),MySqlColumnTypeEnum.MYSQL_TYPE_LONGLONG);
         mySqlColumnTypeEnums.Add(typeof(float),MySqlColumnTypeEnum.MYSQL_TYPE_FLOAT);
         mySqlColumnTypeEnums.Add(typeof(double),MySqlColumnTypeEnum.MYSQL_TYPE_DOUBLE);
         mySqlColumnTypeEnums.Add(typeof(decimal),MySqlColumnTypeEnum.MYSQL_TYPE_NEWDECIMAL);
         mySqlColumnTypeEnums.Add(typeof(string),MySqlColumnTypeEnum.MYSQL_TYPE_STRING);
        CLR_TYPE_TO_MYSQL_COLUMN_TYPES = ImmutableDictionary.CreateRange(mySqlColumnTypeEnums);
    }
}