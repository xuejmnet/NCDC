// using System.Collections.Generic;
// using System.Linq;
// using NCDC.CommandParser.Common.Segment.Generic.Table;
//
// namespace NCDC.CommandParser.Common.Segment.DML
// {
// /*
// * @Author: xjm
// * @Description:
// * @Date: Sunday, 11 April 2021 13:22:14
// * @Email: 326308290@qq.com
// */
//     public sealed class TableReferenceSegment:ISqlSegment
//     {
//         private int _startIndex;
//     
//         private int _stopIndex;
//     
//         private TableFactorSegment _tableFactor;
//         
//         public readonly ICollection<JoinedTableSegment> JoinedTables = new LinkedList<JoinedTableSegment>();
//
//         
//         public int GetStartIndex()
//         {
//             return _startIndex;
//         }
//
//         public void SetStartIndex(int startIndex)
//         {
//             this._startIndex = startIndex;
//         }
//
//         public int GetStopIndex()
//         {
//             return _stopIndex;
//         }
//
//         public void SetStopIndex(int stopIndex)
//         {
//             this._stopIndex = stopIndex;
//         }
//
//         public TableFactorSegment GetTableFactor()
//         {
//             return _tableFactor;
//         }
//
//         public void SetTableFactor(TableFactorSegment tableFactor)
//         {
//             this._tableFactor = tableFactor;
//         }
//         /// <summary>
//         /// 获取所有的表
//         /// </summary>
//         /// <returns></returns>
//         public ICollection<SimpleTableSegment> GetTables() {
//             ICollection<SimpleTableSegment> tables = new LinkedList<SimpleTableSegment>();
//             if (_tableFactor?.GetTable() is SimpleTableSegment simpleTable) {
//                 tables.Add(simpleTable);
//             }
//
//             if (JoinedTables.Any()) {
//                 foreach (var joinedTable in JoinedTables)
//                 {
//                     var t = joinedTable.GetTable();
//                     if (null != t)
//                     {
//                         tables.Add((SimpleTableSegment)t);
//                     }
//                 }
//             }
//             return tables;
//         }
//     }
// }