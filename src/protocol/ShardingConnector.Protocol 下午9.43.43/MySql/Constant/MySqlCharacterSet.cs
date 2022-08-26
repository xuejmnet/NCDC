using System.Collections.Immutable;
using System.Text;

namespace ShardingConnector.Protocol.MySql.Constant;

public class MySqlCharacterSet
{
    public int DbEncoding { get; }
    public Encoding Charset { get; }

    public MySqlCharacterSet(int dbEncoding, Encoding languageEncoding)
    {
        DbEncoding = dbEncoding;
        Charset = languageEncoding;
    }

    public MySqlCharacterSet(int dbEncoding, string encodingName)
    {
        DbEncoding = dbEncoding;
        Charset = Encoding.GetEncoding(encodingName);
    }

    public static MySqlCharacterSet BIG5_CHINESE_CI { get; } = new MySqlCharacterSet(1, "big5");
    public static MySqlCharacterSet LATIN2_CZECH_CS { get; } = new MySqlCharacterSet(2, "latin2");
    public static MySqlCharacterSet UTF8_GENERAL_CI { get; } = new MySqlCharacterSet(33, Encoding.UTF8);
    public static MySqlCharacterSet UTF8MB4_GENERAL_CI { get; } = new MySqlCharacterSet(45, Encoding.UTF8);

    private static readonly ImmutableDictionary<int, MySqlCharacterSet> _characterSets;

    static MySqlCharacterSet()
    {
        _characterSets = ImmutableDictionary.CreateRange<int, MySqlCharacterSet>(
            new Dictionary<int, MySqlCharacterSet>()
            {
                { BIG5_CHINESE_CI.DbEncoding, BIG5_CHINESE_CI },
                { LATIN2_CZECH_CS.DbEncoding, LATIN2_CZECH_CS },
                { UTF8_GENERAL_CI.DbEncoding, UTF8_GENERAL_CI },
                { UTF8MB4_GENERAL_CI.DbEncoding, UTF8MB4_GENERAL_CI }
            });
    }

    public static MySqlCharacterSet FindById(int id)
    {
        if (!_characterSets.TryGetValue(id, out var mySqlCharacterSet))
        {
            throw new NotSupportedException($"character set corresponding to dbEncoding {id} not found");
        }

        return mySqlCharacterSet;
    }
}