namespace ShardingConnector.ProxyServer.Abstractions;

public interface IAdoMethodReplier<T>
{
    LinkedList<Action<T>> Replier { get; }

    /// <summary>
    /// 从新播放target的创建后的动作
    /// </summary>
    /// <param name="target"></param>
    void ReplyTargetMethodInvoke(T target)
    {
        foreach (var action in Replier)
        {
            action?.Invoke(target);
        }
    }

    /// <summary>
    /// 记录target的动作
    /// </summary>
    /// <param name="targetMethod"></param>
    void RecordTargetMethodInvoke(Action<T> targetMethod)
    {
        Replier.AddLast(targetMethod);
    }
}