// using System;
// using System.Collections.Generic;
// using System.Data;
// using System.Data.Common;
// using System.Text;
// using ShardingConnector.AdoNet.AdoNet.Abstraction;
//
// namespace ShardingConnector.AdoNet.AdoNet.Core.Command
// {
//     /*
//     * @Author: xjm
//     * @Description:
//     * @Date: 2021/7/23 15:56:13
//     * @Ver: 1.0
//     * @Email: 326308290@qq.com
//     */
//     public class ShardingParameter : DbParameter, IAdoMethodRecorder<DbParameter>
//     {
//         public ShardingParameterCollection ShardingParameters { get; set; }
//
//         public ShardingParameter():this(null,null)
//         {
//             
//         }
//         public ShardingParameter(string parameterName,object parameterValue)
//         {
//             this._parameterName = parameterName ?? string.Empty;
//             this._parameterValue = parameterValue;
//         }
//
//         public override void ResetDbType()
//         {
//             RecordTargetMethodInvoke(dbParameter => dbParameter.ResetDbType());
//         }
//
//
//         private DbType _dbType;
//         public override DbType DbType
//         {
//             get => _dbType;
//             set
//             {
//                 _dbType = value;
//                 RecordTargetMethodInvoke(dbParameter => dbParameter.DbType = value);
//             }
//         }
//
//         private ParameterDirection _direction;
//         public override ParameterDirection Direction
//         {
//             get => _direction;
//             set
//             {
//                 _direction = value;
//                 RecordTargetMethodInvoke(dbParameter => dbParameter.Direction = value);
//             }
//         }
//
//         private bool _isNullable;
//         public override bool IsNullable
//         {
//             get =>_isNullable;
//             set
//             {
//                 _isNullable = value;
//                 RecordTargetMethodInvoke(dbParameter => dbParameter.IsNullable = value);
//             }
//         }
//
//         private string _parameterName;
//         public override string ParameterName
//         {
//             get => _parameterName;
//             set
//             {
//                 var oldName = _parameterName;
//                 _parameterName = value ==null?null:NormalizeParameterName(value);
//                 ShardingParameters?.ChangeParameterName(this, oldName, _parameterName);
//                 RecordTargetMethodInvoke(dbParameter => dbParameter.ParameterName = value);
//             }
//         }
//         
//         internal static string NormalizeParameterName(string name)
//         {
//             name = name.Trim();
//
//             if ((name.StartsWith("@`", StringComparison.Ordinal) || name.StartsWith("?`", StringComparison.Ordinal)) && name.EndsWith("`", StringComparison.Ordinal))
//                 return name.Substring(2, name.Length - 3).Replace("``", "`");
//             if ((name.StartsWith("@'", StringComparison.Ordinal) || name.StartsWith("?'", StringComparison.Ordinal)) && name.EndsWith("'", StringComparison.Ordinal))
//                 return name.Substring(2, name.Length - 3).Replace("''", "'");
//             if ((name.StartsWith("@\"", StringComparison.Ordinal) || name.StartsWith("?\"", StringComparison.Ordinal)) && name.EndsWith("\"", StringComparison.Ordinal))
//                 return name.Substring(2, name.Length - 3).Replace("\"\"", "\"");
//
//             return name.StartsWith("@", StringComparison.Ordinal) || name.StartsWith("?", StringComparison.Ordinal) ? name.Substring(1) : name;
//         }
//
//         private string _sourceColumn;
//         public override string SourceColumn
//         {
//             get => _sourceColumn;
//             set
//             {
//                 _sourceColumn = value;
//                 RecordTargetMethodInvoke(dbParameter => dbParameter.SourceColumn = value);
//             }
//         }
//
//         private object _parameterValue;
//         public override object Value
//         {
//             get => _parameterValue;
//             set
//             {
//                 _parameterValue = value;
//                 RecordTargetMethodInvoke(dbParameter => dbParameter.Value = value);
//             }
//         }
//
//         private bool _sourceColumnNullMapping;
//         public override bool SourceColumnNullMapping
//         {
//             get => _sourceColumnNullMapping;
//             set
//             {
//                 _sourceColumnNullMapping = value;
//                 RecordTargetMethodInvoke(dbParameter => dbParameter.SourceColumnNullMapping = value);
//             }
//         }
//
//         private int _size;
//         public override int Size
//         {
//             get => _size;
//             set
//             {
//                 _size = value;
//                 RecordTargetMethodInvoke(dbParameter => dbParameter.Size = value);
//             }
//         }
//
//         public override string ToString()
//         {
//             return $"{ParameterName}:{Value}";
//         }
//
//         private event Action<DbParameter> OnRecorder;
//
//         public void ReplyTargetMethodInvoke(DbParameter target)
//         {
//             OnRecorder?.Invoke(target);
//         }
//
//         public void RecordTargetMethodInvoke(Action<DbParameter> targetMethod)
//         {
//             OnRecorder += targetMethod;
//         }
//     }
// }