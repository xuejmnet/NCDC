using System.Text;
using OpenConnector.Extensions;
using NCDC.Sharding.Rewrites.Sql.Token.SimpleObject;

namespace NCDC.Sharding.Rewrites.Sql.Impl
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 13 April 2021 20:51:50
* @Email: 326308290@qq.com
*/
    public abstract class AbstractSqlBuilder:ISqlBuilder
    {
        private readonly SqlRewriteContext _context;

        protected AbstractSqlBuilder(SqlRewriteContext context)
        {
            _context = context;
        }

        public string ToSql() {
            if (_context.GetSqlTokens().IsEmpty()) {
                return _context.GetSql();
            }
            _context.GetSqlTokens().Sort();
            StringBuilder result = new StringBuilder();
            result.Append(_context.GetSql().SubStringWithEndIndex(0, _context.GetSqlTokens()[0].GetStartIndex()));
           
            foreach (var sqlToken in _context.GetSqlTokens())
            {
                result.Append(GetSqlTokenText(sqlToken));
                result.Append(GetConjunctionText(sqlToken));
                
            }
            return result.ToString();
        }
    
        protected abstract string GetSqlTokenText(SqlToken sqlToken);
    
        private string GetConjunctionText(SqlToken sqlToken) {
            return _context.GetSql().SubStringWithEndIndex(GetStartIndex(sqlToken), GetStopIndex(sqlToken));
        }
    
        private int GetStartIndex(SqlToken sqlToken) {
            int startIndex = sqlToken is ISubstitutable substitutable ? substitutable.GetStopIndex() + 1 : sqlToken.GetStartIndex();
            return Math.Min(startIndex, _context.GetSql().Length);
        }
    
        private int GetStopIndex(SqlToken sqlToken) {
            int currentSQLTokenIndex = _context.GetSqlTokens().IndexOf(sqlToken);
            return _context.GetSqlTokens().Count - 1 == currentSQLTokenIndex ? _context.GetSql().Length : _context.GetSqlTokens()[currentSQLTokenIndex + 1].GetStartIndex();
        }
    }
}