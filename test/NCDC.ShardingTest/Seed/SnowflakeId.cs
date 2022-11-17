using System.Text;

namespace NCDC.ShardingTest.Seed;

public class SnowflakeId
{
    // 开始时间截 (new DateTime(2020, 1, 1).ToUniversalTime() - Jan1st1970).TotalMilliseconds
    private const long twepoch = 1577808000000L;

    // 机器id所占的位数
    private const int workerIdBits = 5;

    // 数据标识id所占的位数
    private const int datacenterIdBits = 5;

    // 支持的最大机器id，结果是31 (这个移位算法可以很快的计算出几位二进制数所能表示的最大十进制数) 
    private const long maxWorkerId = -1L ^ (-1L << workerIdBits);

    // 支持的最大数据标识id，结果是31 
    private const long maxDatacenterId = -1L ^ (-1L << datacenterIdBits);

    // 序列在id中占的位数 
    private const int sequenceBits = 12;

    // 数据标识id向左移17位(12+5) 
    private const int datacenterIdShift = sequenceBits + workerIdBits;

    // 机器ID向左移12位 
    private const int workerIdShift = sequenceBits;


    // 时间截向左移22位(5+5+12) 
    private const int timestampLeftShift = sequenceBits + workerIdBits + datacenterIdBits;

    // 生成序列的掩码，这里为4095 (0b111111111111=0xfff=4095) 
    private const long sequenceMask = -1L ^ (-1L << sequenceBits);

    // 数据中心ID(0~31) 
    public long datacenterId { get; private set; }

    // 工作机器ID(0~31) 
    public long workerId { get; private set; }

    // 毫秒内序列(0~4095) 
    public long sequence { get; private set; }

    // 上次生成ID的时间截 
    public long lastTimestamp { get; private set; }


    /// <summary>
    /// 雪花ID
    /// </summary>
    /// <param name="datacenterId">数据中心ID</param>
    /// <param name="workerId">工作机器ID</param>
    public SnowflakeId(long datacenterId, long workerId)
    {
        if (datacenterId > maxDatacenterId || datacenterId < 0)
        {
            throw new Exception(
                string.Format("datacenter Id can't be greater than {0} or less than 0", maxDatacenterId));
        }

        if (workerId > maxWorkerId || workerId < 0)
        {
            throw new Exception(string.Format("worker Id can't be greater than {0} or less than 0", maxWorkerId));
        }

        this.workerId = workerId;
        this.datacenterId = datacenterId;
        this.sequence = 0L;
        this.lastTimestamp = -1L;
    }

    /// <summary>
    /// 获得下一个ID
    /// </summary>
    /// <returns></returns>
    public long NextId()
    {
        lock (this)
        {
            long timestamp = GetCurrentTimestamp();
            if (timestamp > lastTimestamp) //时间戳改变，毫秒内序列重置
            {
                sequence = 0L;
            }
            else if (timestamp == lastTimestamp) //如果是同一时间生成的，则进行毫秒内序列
            {
                sequence = (sequence + 1) & sequenceMask;
                if (sequence == 0) //毫秒内序列溢出
                {
                    timestamp = GetNextTimestamp(lastTimestamp); //阻塞到下一个毫秒,获得新的时间戳
                }
            }
            else //当前时间小于上一次ID生成的时间戳，证明系统时钟被回拨，此时需要做回拨处理
            {
                sequence = (sequence + 1) & sequenceMask;
                if (sequence > 0)
                {
                    timestamp = lastTimestamp; //停留在最后一次时间戳上，等待系统时间追上后即完全度过了时钟回拨问题。
                }
                else //毫秒内序列溢出
                {
                    timestamp = lastTimestamp + 1; //直接进位到下一个毫秒                          
                }
                //throw new Exception(string.Format("Clock moved backwards.  Refusing to generate id for {0} milliseconds", lastTimestamp - timestamp));
            }

            lastTimestamp = timestamp; //上次生成ID的时间截

            //移位并通过或运算拼到一起组成64位的ID
            var id = ((timestamp - twepoch) << timestampLeftShift)
                     | (datacenterId << datacenterIdShift)
                     | (workerId << workerIdShift)
                     | sequence;
            return id;
        }
    }

    /// <summary>
    /// 解析雪花ID
    /// </summary>
    /// <returns></returns>
    public static DateTime AnalyzeIdToDateTime(long Id)
    {
        var timestamp = (Id >> timestampLeftShift);
        var time = Jan1st1970.AddMilliseconds(timestamp + twepoch);
        return time.ToLocalTime();
    }

    /// <summary>
    /// 解析雪花ID
    /// </summary>
    /// <returns></returns>
    public static string AnalyzeId(long Id)
    {
        StringBuilder sb = new StringBuilder();

        var timestamp = (Id >> timestampLeftShift);
        var time = Jan1st1970.AddMilliseconds(timestamp + twepoch);
        sb.Append(time.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss:fff"));

        var datacenterId = (Id ^ (timestamp << timestampLeftShift)) >> datacenterIdShift;
        sb.Append("_" + datacenterId);

        var workerId = (Id ^ ((timestamp << timestampLeftShift) | (datacenterId << datacenterIdShift))) >>
                       workerIdShift;
        sb.Append("_" + workerId);

        var sequence = Id & sequenceMask;
        sb.Append("_" + sequence);

        return sb.ToString();
    }

    /// <summary>
    /// 阻塞到下一个毫秒，直到获得新的时间戳
    /// </summary>
    /// <param name="lastTimestamp">上次生成ID的时间截</param>
    /// <returns>当前时间戳</returns>
    private static long GetNextTimestamp(long lastTimestamp)
    {
        long timestamp = GetCurrentTimestamp();
        while (timestamp <= lastTimestamp)
        {
            timestamp = GetCurrentTimestamp();
        }

        return timestamp;
    }

    /// <summary>
    /// 获取当前时间戳
    /// </summary>
    /// <returns></returns>
    private static long GetCurrentTimestamp()
    {
        return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
    }

    private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
}