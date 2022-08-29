using OpenConnector.Protocol.MySql.Payload;

namespace OpenConnector.Protocol.MySql.Packet.Command;

/// <summary>
/// https://dev.mysql.com/doc/internals/en/com-query-response.html#packet-Protocol::ColumnDefinition41
/// </summary>
public sealed class MySqlColumnDefinition41Packet : IMysqlPacket
{
    private const String CATALOG = "def";

    private const int NEXT_LENGTH = 0x0c;

    private readonly int _sequenceId;

    private readonly int _characterSet;

    private readonly int _flags;

    private readonly String _schema;

    private readonly String _table;

    private readonly String _orgTable;

    private readonly String _name;

    private readonly String _orgName;

    private readonly int _columnLength;

    private readonly int _columnType;

    private readonly int _decimals;

    private readonly bool _containDefaultValues;

    // public MySqlColumnDefinition41Packet( int sequenceId,  ResultSetMetaData resultSetMetaData,  int columnIndex) throws SQLException {
    //     this(sequenceId, MySQLServerInfo.DEFAULT_CHARSET.getId(), resultSetMetaData.getSchemaName(columnIndex), resultSetMetaData.getTableName(columnIndex),
    //             resultSetMetaData.getTableName(columnIndex), resultSetMetaData.getColumnLabel(columnIndex), resultSetMetaData.getColumnName(columnIndex),
    //             resultSetMetaData.getColumnDisplaySize(columnIndex), MySQLBinaryColumnType.valueOfJDBCType(resultSetMetaData.getColumnType(columnIndex)), resultSetMetaData.getScale(columnIndex),
    //             false);
    // }

    /*
     * Field description of column definition Packet.
     *
     * @see <a href="https://github.com/apache/shardingsphere/issues/4358"></a>
     */
    public MySqlColumnDefinition41Packet(int sequenceId, int characterSet, String schema, String table, String orgTable,
        String name, String orgName, int columnLength, int columnType,
        int decimals, bool containDefaultValues) : this(sequenceId, characterSet, 0, schema, table, orgTable, name,
        orgName, columnLength, columnType, decimals, containDefaultValues)
    {
    }

    public MySqlColumnDefinition41Packet(int sequenceId, int characterSet, int flags, String schema, String table,
        String orgTable,
        String name, String orgName, int columnLength, int columnType,
        int decimals, bool containDefaultValues)
    {
        this._sequenceId = sequenceId;
        this._characterSet = characterSet;
        this._flags = flags;
        this._schema = schema;
        this._table = table;
        this._orgTable = orgTable;
        this._name = name;
        this._orgName = orgName;
        this._columnLength = columnLength;
        this._columnType = columnType;
        this._decimals = decimals;
        this._containDefaultValues = containDefaultValues;
    }

    public MySqlColumnDefinition41Packet(MySqlPacketPayload payload)
    {
        _sequenceId = payload.ReadInt1();
        if (!CATALOG.Equals(payload.ReadStringLenenc()))
        {
            throw new ArgumentException(nameof(payload));
        }
        _schema = payload.ReadStringLenenc();
        _table = payload.ReadStringLenenc();
        _orgTable = payload.ReadStringLenenc();
        _name = payload.ReadStringLenenc();
        _orgName = payload.ReadStringLenenc();
        if (NEXT_LENGTH != payload.ReadIntLenenc())
        {
            throw new ArgumentException(nameof(payload));
        }
        _characterSet = payload.ReadInt2();
        _columnLength = payload.ReadInt4();
        _columnType = payload.ReadInt1();
        _flags = payload.ReadInt2();
        _decimals = payload.ReadInt1();
        payload.SkipReserved(2);
        _containDefaultValues = false;
    }

    public void WriteTo(MySqlPacketPayload payload)
    {
        payload.WriteStringLenenc(CATALOG);
        payload.WriteStringLenenc(_schema);
        payload.WriteStringLenenc(_table);
        payload.WriteStringLenenc(_orgTable);
        payload.WriteStringLenenc(_name);
        payload.WriteStringLenenc(_orgName);
        payload.WriteIntLenenc(NEXT_LENGTH);
        payload.WriteInt2(_characterSet);
        payload.WriteInt4(_columnLength);
        payload.WriteInt1(_columnType);
        payload.WriteInt2(_flags);
        payload.WriteInt1(_decimals);
        payload.WriteReserved(2);
        if (_containDefaultValues) {
            payload.WriteIntLenenc(0);
            payload.WriteStringLenenc("");
        }
    }

    public int SequenceId => _sequenceId;
}