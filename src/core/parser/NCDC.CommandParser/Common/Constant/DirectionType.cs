namespace NCDC.CommandParser.Common.Constant;

public class DirectionType
{
    private static readonly ISet<DirectionTypeEnum> ALL_DIRECTION_TYPES = new HashSet<DirectionTypeEnum>(new[]
        { DirectionTypeEnum.ALL, DirectionTypeEnum.FORWARD_ALL, DirectionTypeEnum.BACKWARD_ALL });

    private static readonly ISet<DirectionTypeEnum> FORWARD_COUNT_DIRECTION_TYPES = new HashSet<DirectionTypeEnum>(new[]
    {
        DirectionTypeEnum.NEXT, DirectionTypeEnum.COUNT, DirectionTypeEnum.FORWARD, DirectionTypeEnum.FORWARD_COUNT
    });

    private static readonly ISet<DirectionTypeEnum> BACKWARD_COUNT_DIRECTION_TYPES = new HashSet<DirectionTypeEnum>(
        new[]
        {
            DirectionTypeEnum.PRIOR, DirectionTypeEnum.BACKWARD, DirectionTypeEnum.BACKWARD_COUNT
        });

    public static bool IsAllDirectionType(DirectionTypeEnum directionType)
    {
        return ALL_DIRECTION_TYPES.Contains(directionType);
    }

    public static bool IsForwardCountDirectionType(DirectionTypeEnum directionType)
    {
        return FORWARD_COUNT_DIRECTION_TYPES.Contains(directionType);
    }

    public static bool IsBackwardCountDirectionType(DirectionTypeEnum directionType)
    {
        return BACKWARD_COUNT_DIRECTION_TYPES.Contains(directionType);
    }
}

public enum DirectionTypeEnum
{
    NEXT,
    PRIOR,
    FIRST,
    LAST,
    ABSOLUTE_COUNT,
    RELATIVE_COUNT,
    COUNT,
    ALL,
    FORWARD,
    FORWARD_COUNT,
    FORWARD_ALL,
    BACKWARD,
    BACKWARD_COUNT,
    BACKWARD_ALL,
}