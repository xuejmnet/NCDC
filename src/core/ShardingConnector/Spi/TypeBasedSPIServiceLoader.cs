using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ShardingConnector.Extensions;

namespace ShardingConnector.Spi
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/16 13:11:53
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public abstract class TypeBasedSPIServiceLoader<T>where T:ITypeBasedSpi
    {
        
    
        /**
     * Create new instance for type based SPI.
     * 
     * @param type SPI type
     * @param props SPI properties
     * @return SPI instance
     */
        public  T NewService(string type, IDictionary<string,object> props) {
            ICollection<T> typeBasedServices = LoadTypeBasedServices(type);
            if (typeBasedServices.IsEmpty()) {
                throw new Exception($"Invalid `{typeof(T).Name}` SPI type `{type}`.");
            }
            T result = typeBasedServices.First();
            result.SetProperties(props);
            return result;
        }
    
        /**
     * Create new service by default SPI type.
     *
     * @return type based SPI instance
     */
        public T NewService() {
            T result = LoadFirstTypeBasedService();
            result.SetProperties(new Dictionary<string, object>());
            return result;
        }
    
        private ICollection<T> LoadTypeBasedServices(string type)
        {
            return NewInstanceServiceLoader.NewServiceInstances<T>()
                .Where(input => type.EqualsIgnoreCase(input.GetAlgorithmType())).ToList();
        }
    
        private T LoadFirstTypeBasedService() {
            ICollection<T> instances = NewInstanceServiceLoader.NewServiceInstances<T>();
            if (instances.IsEmpty()) {
                throw new Exception($"Invalid `{typeof(T).Name}` SPI, no implementation class load from SPI.");
            }
            return instances.First();
        }
    }
}