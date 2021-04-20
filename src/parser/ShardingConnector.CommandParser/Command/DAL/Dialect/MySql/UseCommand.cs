namespace ShardingConnector.CommandParser.Command.DAL.Dialect.MySql
{
/*
* @Author: xjm
* @Description:
* @Date: Monday, 12 April 2021 22:18:51
* @Email: 326308290@qq.com
*/
    public sealed class UseCommand:DALCommand
    {
        
        private string schema;

        public void SetSchema(string schema)
        {
            this.schema = schema;
        }

        public string GetSchema()
        {
            return schema;
        }
    }
}