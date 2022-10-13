using NCDC.CommandParser.Common.Segment;

namespace NCDC.CommandParser.Dialect.Command.MySql.Segment;

public sealed class UserSegment : ISqlSegment
{
    public int StartIndex { get; set; }
    public int StopIndex { get; set; }
    public string User { get; set; }
    public string Host { get; set; }
    public string Plugin { get; set; }
    public string Auth { get; set; }
    public string CurrentAuth { get; set; }
    public bool UsesIdentifiedByClause { get; set; }
    public bool UsesIdentifiedWithClause { get; set; }
    public bool UsesAuthenticationStringClause { get; set; }
    public bool UsesReplaceClause { get; set; }
    public bool RetainCurrentPassword { get; set; }
    public bool DiscardOldPassword { get; set; }
    public bool HasPasswordGenerator { get; set; }
    public PasswordOrLockOptionSegment AlterStatus { get; set; }
}