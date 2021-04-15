namespace ShardingConnector.Encrypt.Api
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/14 10:30:21
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public sealed class EncryptColumnRuleConfiguration
    {
        public EncryptColumnRuleConfiguration(string plainColumn, string cipherColumn, string assistedQueryColumn, string encryptor)
        {
            PlainColumn = plainColumn;
            CipherColumn = cipherColumn;
            AssistedQueryColumn = assistedQueryColumn;
            Encryptor = encryptor;
        }

        public  string PlainColumn{get;}
    
        public  string CipherColumn{get;}
    
        public  string AssistedQueryColumn{get;}
    
        public  string Encryptor{get;}
    }
}