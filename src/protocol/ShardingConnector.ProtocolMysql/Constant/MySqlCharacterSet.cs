using System.Collections.Immutable;
using System.Text;

namespace ShardingConnector.ProtocolMysql.Constant;

public class MySqlCharacterSet
{
    public int Id { get; }
    public Encoding Charset { get; }

    public MySqlCharacterSet(int id,Func<Encoding> charsetCreator)
    {
        Id = id;
        try
        {
            Charset = charsetCreator();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            Console.WriteLine(ex.GetType());
        }
    }

    //  public static MySqlCharacterSet BIG5_CHINESE_CI { get; } = new MySqlCharacterSet(1, ()=>Encoding.GetEncoding("big5"));
    // public static MySqlCharacterSet LATIN2_CZECH_CS { get; } = new MySqlCharacterSet(2, ()=>Encoding.GetEncoding("latin2"));
    public static MySqlCharacterSet UTF8_GENERAL_CI { get; } = new MySqlCharacterSet(33, ()=>Encoding.UTF8);
    public static MySqlCharacterSet UTF8MB4_GENERAL_CI { get; } = new MySqlCharacterSet(45, ()=>Encoding.UTF8);

    private static readonly ImmutableDictionary<int, MySqlCharacterSet> _characterSets;

    static MySqlCharacterSet()
    {
        // var mySqlCharacterSets = new Dictionary<int, MySqlCharacterSet>()
        // {
        //     {BIG5_CHINESE_CI.Id,BIG5_CHINESE_CI},
        //     {LATIN2_CZECH_CS.Id,LATIN2_CZECH_CS},
        //     {UTF8MB4_GENERAL_CI.Id,UTF8MB4_GENERAL_CI}
        // };
        // Init(mySqlCharacterSets, BIG5_CHINESE_CI, () => new MySqlCharacterSet(1, Encoding.GetEncoding("big5")));
        // Init(mySqlCharacterSets, LATIN2_CZECH_CS, () => new MySqlCharacterSet(2, Encoding.GetEncoding("latin2")));
        // Init(mySqlCharacterSets, UTF8MB4_GENERAL_CI, () => new MySqlCharacterSet(45, Encoding.UTF8));
        _characterSets = ImmutableDictionary.CreateRange<int, MySqlCharacterSet>(new Dictionary<int, MySqlCharacterSet>()
        {
            // {BIG5_CHINESE_CI.Id,BIG5_CHINESE_CI},
            // {LATIN2_CZECH_CS.Id,LATIN2_CZECH_CS},
            {UTF8_GENERAL_CI.Id,UTF8_GENERAL_CI},
            {UTF8MB4_GENERAL_CI.Id,UTF8MB4_GENERAL_CI}
        });
    }

    public static MySqlCharacterSet FindById(int id)
    {
        if (!_characterSets.TryGetValue(id, out var mySqlCharacterSet))
        {
            throw new NotSupportedException($"character set corresponding to id {id} not found");
        }

        return mySqlCharacterSet;
    }
}