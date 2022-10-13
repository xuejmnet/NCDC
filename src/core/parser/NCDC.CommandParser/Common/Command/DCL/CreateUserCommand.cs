using System;
using NCDC.CommandParser.Dialect.Command.MySql.Segment;

namespace NCDC.CommandParser.Common.Command.DCL
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 20 April 2021 21:27:54
* @Email: 326308290@qq.com
*/
    public abstract class CreateUserCommand:AbstractSqlCommand,IDCLCommand
    {
        public ICollection<UserSegment> Users = new LinkedList<UserSegment>();
    }
}