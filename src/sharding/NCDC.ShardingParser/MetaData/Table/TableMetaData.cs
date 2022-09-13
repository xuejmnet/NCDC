// using NCDC.Basic.TableMetadataManagers;
// using NCDC.ShardingParser.MetaData.Index;
// using NCDC.DataStructure;
//
// namespace NCDC.ShardingParser.MetaData.Table
// {
// /*
// * @Author: xjm
// * @Description:
// * @Date: Thursday, 08 April 2021 21:55:23
// * @Email: 326308290@qq.com
// */
//     public sealed class TableMetaData
//     {
//         private readonly IDictionary<string, ColumnMetadata> _columns;
//
//         private readonly IDictionary<string, IndexMetaData> _indexes;
//
//         private readonly List<string> _columnNames = new List<string>();
//
//         private readonly List<string> _primaryKeyColumns = new List<string>();
//
//         public TableMetaData(ICollection<ColumnMetadata> columnMetadataList, ICollection<IndexMetaData> indexMetaDataList)
//         {
//             _columns = CreateColumns(columnMetadataList);
//             _indexes = CreateIndexes(indexMetaDataList);
//         }
//
//         private IDictionary<string, ColumnMetadata> CreateColumns(ICollection<ColumnMetadata> columnMetadataList)
//         {
//             IDictionary<string, ColumnMetadata> result = new LinkedDictionary<string, ColumnMetadata>();
//             foreach (var columnMetaData in columnMetadataList)
//             {
//                 var lowerColumnName = columnMetaData.Name.ToLower();
//                 _columnNames.Add(lowerColumnName);
//                 result.Add(lowerColumnName, columnMetaData);
//                 if (columnMetaData.PrimaryKey)
//                 {
//                     _primaryKeyColumns.Add(lowerColumnName);
//                 }
//             }
//
//             return result;
//         }
//
//         private bool Equals(TableMetaData other)
//         {
//             return _columns.SequenceEqual(other._columns) && _indexes.SequenceEqual(other._indexes) &&
//                    _columnNames.SequenceEqual(other._columnNames) &&
//                    _primaryKeyColumns.SequenceEqual(other._primaryKeyColumns);
//         }
//
//         public override bool Equals(object obj)
//         {
//             return ReferenceEquals(this, obj) || obj is TableMetaData other && Equals(other);
//         }
//
//         public override int GetHashCode()
//         {
//             unchecked
//             {
//                 var hashCode = (_columns != null ? _columns.GetHashCode() : 0);
//                 hashCode = (hashCode * 397) ^ (_indexes != null ? _indexes.GetHashCode() : 0);
//                 hashCode = (hashCode * 397) ^ (_columnNames != null ? _columnNames.GetHashCode() : 0);
//                 hashCode = (hashCode * 397) ^ (_primaryKeyColumns != null ? _primaryKeyColumns.GetHashCode() : 0);
//                 return hashCode;
//             }
//         }
//
//         private IDictionary<string, IndexMetaData> CreateIndexes(ICollection<IndexMetaData> indexMetaDataList)
//         {
//             IDictionary<string, IndexMetaData> result = new LinkedDictionary<string, IndexMetaData>();
//             foreach (var indexMetaData in indexMetaDataList)
//             {
//                 result.Add(indexMetaData.Name.ToLower(), indexMetaData);
//             }
//
//             return result;
//         }
//
//         public IDictionary<string, ColumnMetadata> GetColumns()
//         {
//             return _columns;
//         }
//
//         public IDictionary<string, IndexMetaData> GetIndexes()
//         {
//             return _indexes;
//         }
//         /**
//      * Get column meta data.
//      *
//      * @param columnIndex column index
//      * @return column meta data
//      */
//         public ColumnMetadata GetColumnMetaData(int columnIndex)
//         {
//             if (_columns.TryGetValue(_columnNames[columnIndex], out var value))
//             {
//                 return value;
//             }
//
//             return null;
//         }
//
//         /**
//      * Find index of column.
//      *
//      * @param columnName column name
//      * @return index of column if found, otherwise -1
//      */
//         public int FindColumnIndex(string columnName)
//         {
//             for (int i = 0; i < _columnNames.Count; i++)
//             {
//                 if (_columnNames[i].Equals(columnName))
//                 {
//                     return i;
//                 }
//             }
//
//             return -1;
//         }
//
//         /**
//      * Judge column whether primary key.
//      *
//      * @param columnIndex column index
//      * @return true if the column is primary key, otherwise false
//      */
//         public bool IsPrimaryKey(int columnIndex)
//         {
//             if (columnIndex >= _columnNames.Count)
//             {
//                 return false;
//             }
//
//             if (_columns.TryGetValue(_columnNames[columnIndex], out var value))
//             {
//                 return value.PrimaryKey;
//             }
//
//             return false;
//         }
//     }
// }