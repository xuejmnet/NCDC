using System.Data.Common;

namespace OpenConnector.Transaction.Core
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/16 13:53:39
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public sealed class ResourceDbProviderFactory
    {
        public  string OriginalName { get; }
    
        public string UniqueResourceName{ get; }
    
        public  DbProviderFactory DbProviderFactory{ get; }
    
        public ResourceDbProviderFactory(string originalName, DbProviderFactory dbProviderFactory) {
            this.OriginalName = originalName;
            this.DbProviderFactory = dbProviderFactory;
            this.UniqueResourceName = ResourceIDGenerator.GetInstance().NextId() + originalName;
        }
    }
}