using System;
using System.Collections.Generic;
using System.Text;

namespace ShardingConnector.Common.Config
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/13 14:56:53
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class DatabaseAccessConfiguration
    {
        public DatabaseAccessConfiguration(string url, string userName, string password)
        {
            Url = url;
            UserName = userName;
            Password = password;
        }

        public string Url { get; }

        public string UserName { get; }

        public string Password { get; }

    }
}
