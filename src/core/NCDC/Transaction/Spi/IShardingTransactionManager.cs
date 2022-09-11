using System.Collections.ObjectModel;
using System.Data.Common;
using OpenConnector.Spi.DataBase.DataBaseType;
using OpenConnector.Transaction.Core;

namespace OpenConnector.Transaction.Spi
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/16 00:00:00
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    /// <summary>
    /// 
    /// </summary>
    public interface IShardingTransactionManager
    {
        /**
     * Initialize sharding transaction manager.
     *
     * @param databaseType database type
     * @param resourceDataSources resource data sources
     */
        void Init(IDatabaseType databaseType, Collection<ResourceDbProviderFactory> resourceDataSources);
    
        /**
     * Get transaction type.
     *
     * @return transaction type
     */
        TransactionTypeEnum getTransactionType();
    
        /**
     * Judge is in transaction or not.
     * 
     * @return in transaction or not
     */
        bool IsInTransaction();
    
        /**
     * Get transactional connection.
     *
     * @param dataSourceName data source name
     * @return connection
     * @throws SQLException SQL exception
     */
       DbConnection getConnection(string dataSourceName);
    
        /**
     * Begin transaction.
     */
        void Begin();
    
        /**
     * Commit transaction.
     */
        void Commit();
    
        /**
     * Rollback transaction.
     */
        void Rollback();
    }
}