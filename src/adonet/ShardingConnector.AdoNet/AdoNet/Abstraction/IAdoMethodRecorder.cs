using System;
using System.Data.Common;

/*
* @Author: xjm
* @Description:
* @Date: DATE TIME
* @Email: 326308290@qq.com
*/
namespace ShardingConnector.AdoNet.AdoNet.Abstraction
{
    public interface IAdoMethodRecorder<T>
    {
         event Action<T> OnRecorder;

         /// <summary>
         /// 从新播放target的创建后的动作
         /// </summary>
         /// <param name="target"></param>
         void ReplyTargetMethodInvoke(T target);

        /// <summary>
        /// 记录target的动作
        /// </summary>
        /// <param name="targetMethod"></param>
        void RecordTargetMethodInvoke(Action<T> targetMethod);
    }
}