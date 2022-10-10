using NCDC.ProxyServer.Connection.Abstractions;

namespace NCDC.ProxyServer.Connection;

public interface IAdoMethodReplier
{
    LinkedList<Func<IServerDbConnection,Task>> ServerDbConnectionInvokeReplier { get; }

    /// <summary>
    /// 从新播放target的创建后的动作
    /// </summary>
    /// <param name="target"></param>
   public async Task ReplyTargetMethodInvokeAsync(IServerDbConnection target)
    {
        foreach (var action in ServerDbConnectionInvokeReplier)
        {
            await action.Invoke(target);
        }
    }

    /// <summary>
    /// 记录target的动作
    /// </summary>
    /// <param name="targetMethod"></param>
    public void RecordTargetMethodInvoke(Func<IServerDbConnection,Task> targetMethod)
    {
        ServerDbConnectionInvokeReplier.AddLast(targetMethod);
    }
}