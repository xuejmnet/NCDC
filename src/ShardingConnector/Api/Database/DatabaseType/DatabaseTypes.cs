using System.Collections.Generic;
using System.Linq;
using ShardingConnector.Exceptions;
using ShardingConnector.Spi.DataBase.DataBaseType;

namespace ShardingConnector.Api.Database.DatabaseType
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/14 11:33:11
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public sealed class DatabaseTypes
    {
        
    private static readonly IDictionary<string, IDatabaseType> DATABASE_TYPES = new Dictionary<string, IDatabaseType>();
    
    static DatabaseTypes()
    {
        var databaseTypes = ServiceLoader.Load<IDatabaseType>();
        foreach (var databaseType in databaseTypes)
        {
            DATABASE_TYPES.Add(databaseType.GetName(),databaseType);
        }
    }
    
    /**
     * Get name of trunk database type.
     * 
     * @param databaseType database type
     * @return name of trunk database type
     */
    public static string GetTrunkDatabaseTypeName(IDatabaseType databaseType) {
        return databaseType is IBranchDatabaseType branchDatabaseType ? branchDatabaseType.GetTrunkDatabaseType().GetName() : databaseType.GetName();
    }
    
    /**
     * Get trunk database type.
     *
     * @param name database name 
     * @return trunk database type
     */
    public static IDatabaseType GetTrunkDatabaseType(string name) {
        return DATABASE_TYPES[name] is IBranchDatabaseType branchDatabaseType ? branchDatabaseType.GetTrunkDatabaseType() : GetActualDatabaseType(name);
    }
    
    /**
     * Get actual database type.
     *
     * @param name database name 
     * @return actual database type
     */
    public static IDatabaseType GetActualDatabaseType(string name)
    {
        var type = DATABASE_TYPES[name];
        if (type == null)
            throw new ShardingException($"Unsupported database:'{name}'");
        return  type;
    }
    
    /**
     * Get database type by URL.
     * 
     * @param url database URL
     * @return database type
     */
    public static IDatabaseType GetDatabaseTypeByUrl(string url) {
        return  DATABASE_TYPES.Where(o=>MatchStandardURL(url,o.Value)||MatchURLAlias(url,o.Value)).Select(o=>o.Value).FirstOrDefault()??DATABASE_TYPES["Sql92"];
    }
    
    private static bool MatchStandardURL(string url, IDatabaseType databaseType) {
        return url.StartsWith($"adonet:{databaseType.GetName().ToLower()}:");
    }
    
    private static bool MatchURLAlias(string url, IDatabaseType databaseType) {
        return databaseType.GetAdoNetUrlPrefixAlias().Any(url.StartsWith);
    }
    }
}