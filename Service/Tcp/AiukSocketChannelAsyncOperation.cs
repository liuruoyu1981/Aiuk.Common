using System.Net.Sockets;

namespace Aiuk.Common.Service.Tcp
{
    /// <summary>
    /// 异步Socket操作。
    /// </summary>
    public class AiukSocketChannelAsyncOperation : SocketAsyncEventArgs
    {

        /// <summary>
        /// 验证通信状态。
        /// </summary>
        public void Validate()
        {
            var socketError = SocketError;
            if (socketError != SocketError.Success)
            {
                throw new SocketException((int)socketError);
            }
        }


    }
}
