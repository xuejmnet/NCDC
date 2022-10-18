using NCDC.CommandParser.Common.Command.DML;
using NCDC.CommandParser.Common.Segment.DML.Expr;
using NCDC.CommandParser.Common.Segment.DML.Expr.Simple;
using NCDC.Basic.TableMetadataManagers;
using NCDC.CommandParser.Common.Segment.DML.Assignment;
using NCDC.CommandParser.Dialect.Handler.DML;
using NCDC.Exceptions;
using NCDC.Extensions;
using NCDC.ShardingAdoNet;

namespace NCDC.ShardingParser.Segment.Insert.Keygen.Engine
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/13 9:10:24
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class GeneratedKeyContextEngine
    {
        private readonly InsertCommand _insertCommand;
        private readonly ITableMetadataManager _tableMetadataManager;

        public GeneratedKeyContextEngine(InsertCommand insertCommand,ITableMetadataManager tableMetadataManager)
        {
            _insertCommand = insertCommand;
            _tableMetadataManager = tableMetadataManager;
        }

        /**
     * Create generate key context.
     *
     * @param parameters SQL parameters
     * @param insertStatement insert statement
     * @return generate key context
     */
        public GeneratedKeyContext? CreateGenerateKeyContext(List<string> insertColumnNames,List<List<IExpressionSegment>> valueExpressions,ParameterContext parameterContext)
        {
            string tableName = _insertCommand.Table!.TableName.IdentifierValue.Value;
            var generateKeyColumnName = FindGenerateKeyColumn(tableName);
            if (generateKeyColumnName != null)
            {
                if (ContainsGenerateKey(insertColumnNames, generateKeyColumnName))
                {
                    return FindGeneratedKey(insertColumnNames, valueExpressions,parameterContext, generateKeyColumnName);
                }

                return new GeneratedKeyContext(generateKeyColumnName, true);
            }

            return null;
        }

        private string? FindGenerateKeyColumn(string tableName)
        {
            var tableMetadata = _tableMetadataManager.TryGet(tableName);
            if (tableMetadata==null)
            {
                return null;
            }
            foreach (var columnKv in tableMetadata.Columns)
            {
                if (columnKv.Value.Generated)
                {
                    return columnKv.Key;
                }
            }
            return null;
        }

        private bool ContainsGenerateKey(List<string> insertColumnNames, string generateKeyColumnName)
        {
            if (insertColumnNames.IsEmpty())
            {
                return _tableMetadataManager.GetAllColumnNames(_insertCommand.Table!.TableName.IdentifierValue.Value)
                    .Count == GetValueCountForPerGroup();
            }
            return insertColumnNames.Contains(generateKeyColumnName);
        }
        private int GetValueCountForPerGroup() {
            if (_insertCommand.Values.IsNotEmpty()) {
                return _insertCommand.Values.First().Values.Count;
            }
            var setAssignment = InsertCommandHandler.GetSetAssignmentSegment(_insertCommand);
            if (setAssignment is not null)
            {
                return setAssignment.Assignments.Count;
            }
            if (_insertCommand.InsertSelect is not null) {
                return _insertCommand.InsertSelect.Select.Projections?.Projections.Count??0;
            }
            return 0;
        }
        /// <summary>
        /// 找到自动生成的键比如自增id
        /// </summary>
        /// <param name="insertColumnNames"></param>
        /// <param name="valueExpressions"></param>
        /// <param name="parameterContext"></param>
        /// <param name="generateKeyColumnName"></param>
        /// <returns></returns>
        /// <exception cref="ShardingException"></exception>
        private GeneratedKeyContext FindGeneratedKey(List<string> insertColumnNames,List<List<IExpressionSegment>> valueExpressions,ParameterContext parameterContext, string generateKeyColumnName)
        {
            GeneratedKeyContext result = new GeneratedKeyContext(generateKeyColumnName, false);
            foreach (var expression in FindGenerateKeyExpressions(insertColumnNames,valueExpressions, generateKeyColumnName))
            {
                if (expression is ParameterMarkerExpressionSegment parameterMarkerExpressionSegment)
                {
                    if (parameterContext.IsEmpty())
                    {
                        continue;
                    }
                    var parameterName = parameterMarkerExpressionSegment.ParamName;


                    if (!parameterContext.TryGetParameterValue(parameterName,out var value))
                    {
                        throw new ShardingException($"not found parameter name:[{parameterName}] in parameters");
                    }
                   
                    result.GetGeneratedValues().Add((IComparable)value);
                }
                else if (expression is LiteralExpressionSegment literalExpressionSegment)
                {
                    result.GetGeneratedValues().Add((IComparable)literalExpressionSegment.Literals);
                }
            }
            return result;
        }

        private ICollection<IExpressionSegment> FindGenerateKeyExpressions(List<string> insertColumnNames,List<List<IExpressionSegment>> valueExpressions, string generateKeyColumnName)
        {
            ICollection<IExpressionSegment> result = new LinkedList<IExpressionSegment>();
            foreach (var expression in valueExpressions)
            {
                result.Add(expression[FindGenerateKeyIndex(insertColumnNames, generateKeyColumnName.ToLower())]);
            }
            return result;
        }

        private int FindGenerateKeyIndex(List<string> insertColumnNames, string generateKeyColumnName)
        {
            if (insertColumnNames.IsEmpty())
            {
                return _tableMetadataManager
                    .GetAllColumnNames(_insertCommand.Table!.TableName.IdentifierValue.Value).ToList()
                    .IndexOf(generateKeyColumnName);
            }

            return insertColumnNames.IndexOf(generateKeyColumnName);
        }
    }
}
