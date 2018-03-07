using System;

namespace Aiuk.Common.Service.Tcp
{
    /// <summary>
    /// 频道接口，通信通道的抽象表示。
    /// </summary>
    public interface IAiukChannel : IComparable<IAiukChannel>
    {
        /// <summary>
        /// 频道编号。
        /// </summary>
        IAiukChannelId Id { get; }

#if NET35

        /// <summary>
        /// 异步写入并冲刷数据缓冲区。
        /// </summary>
        /// <param name="message">消息对象。</param>
        void WriteAndFlushAsync(object message);



#endif



    }
}

