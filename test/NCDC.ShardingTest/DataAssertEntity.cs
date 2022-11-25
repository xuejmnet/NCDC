using System.Collections;
using System.Data.Common;

namespace NCDC.ShardingTest;

public class DataAssertEntity:IEquatable<DataAssertEntity>
{
    private readonly int _columnLength;
    private readonly List<List<object?>> _columns = new ();

    public DataAssertEntity(int columnLength)
    {
        _columnLength = columnLength;
    }

    public void Add(DbDataReader dataReader)
    {
        var columnValues = new List<object?>(_columnLength);
        for (int i = 0; i < _columnLength; i++)
        {
            columnValues.Add(dataReader[i]);
        }
        _columns.Add(columnValues);
    }

    public bool Equals(DataAssertEntity? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _columnLength == other._columnLength && SequenceEqualElement(other._columns);
    }

    private bool SequenceEqualElement(List<List<object?>> columns)
    {
        if (_columns.Count != columns.Count)
        {
            return false;
        }
        for (int i = 0; i < _columns.Count; i++)
        {
            for (int j = 0; j < _columnLength; j++)
            {
                if (_columns[i][j] == null && columns[i][j] == null)
                {
                    continue;
                }
                else
                {
                    if (_columns[i][j] == null || columns[i][j] == null)
                    {
                        return false;
                    }
                }

                var obj1 = _columns[i][j]!;
                var obj2 = columns[i][j]!;
                if (obj1.GetType() != obj2.GetType())
                {
                    return false;
                }

                if (obj1 is IEnumerable enumerable1)
                {
                    var x = new List<object>();
                    var y = new List<object>();
                    var enumerable2 = (IEnumerable)obj2;
                    foreach (var o in enumerable1)
                    {
                        x.Add(0);
                    }
                    foreach (var o in enumerable2)
                    {
                        y.Add(0);
                    }

                    if (!x.SequenceEqual(y))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!obj1.Equals(obj2))
                    {
                        return false;
                    }
                }

            }
            // if (!_columns[i].SequenceEqual(columns[i]))
            // {
            //     return false;
            // }
        }

        return true;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((DataAssertEntity)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_columnLength, _columns);
    }
}