using System.Collections.Generic;

namespace OpenConnector.Spi
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
    public interface ITypeBasedSpi
    {
        /**
     * Get algorithm type.
     * 
     * @return type
     */
        string GetAlgorithmType();
    
        /**
     * Get properties.
     * 
     * @return properties of algorithm
     */
        IDictionary<string,object> GetProperties();
    
        /**
     * Set properties.
     * 
     * @param properties properties of algorithm
     */
        void SetProperties(IDictionary<string,object> properties);
    }
}