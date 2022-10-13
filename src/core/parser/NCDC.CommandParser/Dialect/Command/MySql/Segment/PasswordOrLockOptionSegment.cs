using NCDC.CommandParser.Common.Segment;

namespace NCDC.CommandParser.Dialect.Command.MySql.Segment;

public sealed class PasswordOrLockOptionSegment : ISqlSegment
{
    public int StartIndex { get; set; }
    public int StopIndex { get; set; }
    public bool UpdatePasswordExpiredFields { get; set; }
    public bool UpdatePasswordExpiredColumn { get; set; }
    public bool UseDefaultPasswordLifeTime { get; set; }
    public int ExpireAfterDays { get; set; }
    public bool UpdateAccountLockedColumn { get; set; }
    public bool AccountLocked { get; set; }
    public int PasswordHistoryLength { get; set; }
    public bool UseDefaultPasswordHistory { get; set; }
    public bool UpdatePasswordHistory { get; set; }
    public int PasswordReuseInterval { get; set; }
    public bool UseDefaultPasswordReuseInterval { get; set; }
    public bool UpdatePasswordReuseInterval { get; set; }
    public int FailedLoginAttempts { get; set; }
    public bool UpdateFailedLoginAttempts { get; set; }
    public int PasswordLockTime { get; set; }
    public bool UpdatePasswordLockTime { get; set; }
    public ACLAttributeEnum UpdatePasswordRequireCurrent { get; set; }
}