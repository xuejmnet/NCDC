namespace NCDC.CommandParser.Common.Constant;

public enum TransactionIsolationLevelEnum
{
    NONE,
    READ_UNCOMMITTED,
    READ_COMMITTED,
    REPEATABLE_READ,
    SNAPSHOT,
    SERIALIZABLE
}