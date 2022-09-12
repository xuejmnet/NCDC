using System;
using System.Collections.Generic;
using System.Text;
using NCDC.Exceptions;

namespace OpenConnector.Base
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/24 14:50:50
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public class ShardingAssert
    {
        private ShardingAssert()
        {
            
        }
       /// <summary>
     /// Checks the argument passed in. if it's null, it throws an ArgumentNullException
     /// </summary>
     /// <param name="argument">The argument.</param>
     /// <param name="name">The name.</param>
        public static void ShouldBeNotNull(object argument, string name)
        {
            if (argument == null)
            {
                throw new ShardingAssertException(name);
            }
        }
        public static void ShouldBeNull(object argument, string name)
        {
            if (argument != null)
            {
                throw new ShardingAssertException(name);
            }
        }


        /// <summary>
        /// Checks if the argument returns true with the func passed in. If not, it throws an ArgumentException with the error message specified. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="checkFunc">The check func.</param>
        /// <param name="argument">The argument.</param>
        /// <param name="formattedError">The formatted error message.</param>
        public static void ShouldBeTrue<T>(Func<T, bool> checkFunc, T argument, string formattedError)
        {
            ShouldBeNotNull(checkFunc, "checkFunc");
            if (!checkFunc(argument))
            {
                throw new ShardingAssertException(formattedError);
            }
        }

        public static void ShouldBeTrue(bool check, string msg)
        {
            if (!check)
                throw new ShardingAssertException(msg);
        }
        public static void ShouldBeFalse(bool check, string msg)
        {
            if (!check)
                throw new ShardingAssertException(msg);
        }
        public static void If(bool check, string msg)
        {
            if (check)
                throw new ShardingAssertException(msg);
        }
        public static void Else(bool check, string msg)
        {
            if (!check)
                throw new ShardingAssertException(msg);
        }
    }
}
