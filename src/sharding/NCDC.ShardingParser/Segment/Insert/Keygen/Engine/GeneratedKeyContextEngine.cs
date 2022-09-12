using NCDC.CommandParser.Command.DML;
using NCDC.CommandParser.Segment.DML.Expr;
using NCDC.CommandParser.Segment.DML.Expr.Simple;
using NCDC.ShardingParser.MetaData;
using NCDC.ShardingParser.MetaData.Schema;
using NCDC.Basic.TableMetadataManagers;
using NCDC.Exceptions;
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
        private readonly ITableMetadataManager _tableMetadataManager;

        public GeneratedKeyContextEngine(ITableMetadataManager tableMetadataManager)
        {
            _tableMetadataManager = tableMetadataManager;
        }

        /**
     * Create generate key context.
     *
     * @param parameters SQL parameters
     * @param insertStatement insert statement
     * @return generate key context
     */
        public GeneratedKeyContext CreateGenerateKeyContext(ParameterContext parameterContext, InsertCommand insertCommand)
        {
            string tableName = insertCommand.Table.GetTableName().GetIdentifier().GetValue();
            var generateKeyColumnName = FindGenerateKeyColumn(tableName);
            if (generateKeyColumnName != null)
            {
                if (ContainsGenerateKey(insertCommand, generateKeyColumnName))
                {
                    return FindGeneratedKey(parameterContext, insertCommand, generateKeyColumnName);
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

        private bool ContainsGenerateKey(InsertCommand insertCommand, string generateKeyColumnName)
        {
            if (!insertCommand.GetColumnNames().Any())
            {
                return _tableMetadataManager.GetAllColumnNames(insertCommand.Table.GetTableName().GetIdentifier().GetValue())
                    .Count == insertCommand.GetValueCountForPerGroup();
            }
            return insertCommand.GetColumnNames().Contains(generateKeyColumnName);
        }
        /// <summary>
        /// 找到自动生成的键比如自增id
        /// </summary>
        /// <param name="parameterContext"></param>
        /// <param name="insertCommand"></param>
        /// <param name="generateKeyColumnName"></param>
        /// <returns></returns>
        /// <exception cref="ShardingException"></exception>
        private GeneratedKeyContext FindGeneratedKey(ParameterContext parameterContext, InsertCommand insertCommand, string generateKeyColumnName)
        {
            GeneratedKeyContext result = new GeneratedKeyContext(generateKeyColumnName, false);
            foreach (var expression in FindGenerateKeyExpressions(insertCommand, generateKeyColumnName))
            {
                if (expression is ParameterMarkerExpressionSegment parameterMarkerExpressionSegment)
                {
                    var parameterName = parameterMarkerExpressionSegment.GetParameterName();


                    if (!parameterContext.TryGetParameterValue(parameterName,out var value))
                    {
                        throw new ShardingException($"not found parameter name:[{parameterName}] in parameters");
                    }
                   
                    result.GetGeneratedValues().Add((IComparable)value);
                }
                else if (expression is LiteralExpressionSegment literalExpressionSegment)
                {
                    result.GetGeneratedValues().Add((IComparable)literalExpressionSegment.GetLiterals());
                }
            }
            return result;
        }

        private ICollection<IExpressionSegment> FindGenerateKeyExpressions(InsertCommand insertCommand, string generateKeyColumnName)
        {
            ICollection<IExpressionSegment> result = new LinkedList<IExpressionSegment>();
            foreach (var expression in insertCommand.GetAllValueExpressions())
            {
                result.Add(expression[FindGenerateKeyIndex(insertCommand, generateKeyColumnName.ToLower())]);
            }
            return result;
        }

        private int FindGenerateKeyIndex(InsertCommand insertCommand, string generateKeyColumnName)
        {
            if (!insertCommand.GetColumnNames().Any())
            {
                return _tableMetadataManager
                    .GetAllColumnNames(insertCommand.Table.GetTableName().GetIdentifier().GetValue()).ToList()
                    .IndexOf(generateKeyColumnName);
            }
            return insertCommand.GetColumnNames().IndexOf(generateKeyColumnName);
        }
    }
}
