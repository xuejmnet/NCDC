using Microsoft.Extensions.Logging;
using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParserBinder.Command;
using OpenConnector.Logger;
using OpenConnector.Sharding.Executors.Context;

namespace OpenConnector.Sharding.Executors.SqlLog
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/13 14:09:48
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    public delegate void Log(string msg);
    public class SqlLogger
    {
        private static readonly ILogger<SqlLogger> _logger = InternalLoggerFactory.CreateLogger<SqlLogger>();

        private SqlLogger()
        {
            
        }

        /// <summary>
        /// 记录sql
        /// </summary>
        /// <param name="logicSql"></param>
        /// <param name="showSimple">简单记录</param>
        /// <param name="sqlCommandContext"></param>
        /// <param name="executionUnits"></param>
        public static void LogSql(string logicSql, bool showSimple, ISqlCommandContext<ISqlCommand> sqlCommandContext, ICollection<ExecutionUnit> executionUnits)
        {
            _logger.LogInformation($"Logic SQL: {logicSql}");
            _logger.LogInformation($"SqlCommand: {sqlCommandContext.GetSqlCommand()}");
            if (showSimple)
            {
                LogSimpleMode(executionUnits);
            }
            else
            {
                LogNormalMode(executionUnits);
            }
        }

        private static void LogSimpleMode(ICollection<ExecutionUnit> executionUnits)
        {
            ISet<string> dataSourceNames = new HashSet<string>();
            foreach (var executionUnit in executionUnits)
            {
                dataSourceNames.Add(executionUnit.GetDataSourceName());

            }
            _logger.LogInformation($"Actual SQL(simple): {dataSourceNames} ::: {executionUnits.Count}");
        }

        private static void LogNormalMode(ICollection<ExecutionUnit> executionUnits)
        {
            foreach (var executionUnit in executionUnits)
            {
                if (executionUnit.GetSqlUnit().GetParameterContext().IsEmpty())
                {
                    _logger.LogInformation($"Actual SQL: {executionUnit.GetDataSourceName()} ::: {executionUnit.GetSqlUnit().GetSql()}");
                }
                else
                {
                    _logger.LogInformation($"Actual SQL: {executionUnit.GetDataSourceName()} ::: {executionUnit.GetSqlUnit().GetSql()} ::: {string.Join(",", executionUnit.GetSqlUnit().GetParameterContext().GetDbParameters().Select(o=>o.ToString()))}");
                }
            }
        }
    }
}
