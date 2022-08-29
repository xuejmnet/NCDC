using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace OpenConnector.Spi.DataBase.DataBaseType.DataBaseDiscover
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/25 17:30:05
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class DataBaseTypeDiscoverManager
    {
        private static readonly DataBaseTypeDiscoverManager Instance;
        private readonly ICollection<IDataBaseTypeDiscover> _dataBaseTypeDiscovers = NewInstanceServiceLoader.NewServiceInstances<IDataBaseTypeDiscover>();
        static DataBaseTypeDiscoverManager()
        {
            NewInstanceServiceLoader.Register<IDataBaseTypeDiscover>();
            Instance = new DataBaseTypeDiscoverManager();
        }

        private DataBaseTypeDiscoverManager()
        {
            
        }

        public static DataBaseTypeDiscoverManager GetInstance()
        {
            return Instance;
        }

        public  string MatchName(DbConnection dbConnection)
        {
            foreach (var dataBaseTypeDiscover in _dataBaseTypeDiscovers)
            {
                if (dataBaseTypeDiscover.Match(dbConnection))
                    return dataBaseTypeDiscover.DataBaseTypeName;
            }

            return null;
        }


    }
}
