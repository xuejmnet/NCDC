using System.Collections.Immutable;
using System.Text;

namespace NCDC.Protocol.MySql.Constant;

public class MySqlCharacterSet1
{
    public int DbEncoding { get; }
    public Encoding Charset { get; }

    public MySqlCharacterSet1(int dbEncoding, Encoding languageEncoding)
    {
        DbEncoding = dbEncoding;
        Charset = languageEncoding;
    }

    public MySqlCharacterSet1(int dbEncoding, string encodingName)
    {
        DbEncoding = dbEncoding;
        Charset = Encoding.GetEncoding(encodingName);
    }

    public static MySqlCharacterSet1 BIG5_CHINESE_CI { get; } = new MySqlCharacterSet1(1, "big5");
    public static MySqlCharacterSet1 LATIN2_CZECH_CS { get; } = new MySqlCharacterSet1(2, "latin2");
    public static MySqlCharacterSet1 UTF8_GENERAL_CI { get; } = new MySqlCharacterSet1(33, Encoding.UTF8);
    public static MySqlCharacterSet1 UTF8MB4_GENERAL_CI { get; } = new MySqlCharacterSet1(45, Encoding.UTF8);

    private static readonly ImmutableDictionary<int, MySqlCharacterSet1> _characterSets;

    static MySqlCharacterSet1()
    {
        _characterSets = ImmutableDictionary.CreateRange<int, MySqlCharacterSet1>(
            new Dictionary<int, MySqlCharacterSet1>()
            {
                { BIG5_CHINESE_CI.DbEncoding, BIG5_CHINESE_CI },
                { LATIN2_CZECH_CS.DbEncoding, LATIN2_CZECH_CS },
                { UTF8_GENERAL_CI.DbEncoding, UTF8_GENERAL_CI },
                { UTF8MB4_GENERAL_CI.DbEncoding, UTF8MB4_GENERAL_CI }
            });
    }

    public static MySqlCharacterSet1 FindById(int id)
    {
        if (!_characterSets.TryGetValue(id, out var MySqlCharacterSet1))
        {
            throw new NotSupportedException($"character set corresponding to dbEncoding {id} not found");
        }

        return MySqlCharacterSet1;
    }
}