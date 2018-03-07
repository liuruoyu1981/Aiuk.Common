namespace Aiuk.Common.Service.Tcp
{
    /// <summary>
    /// 频道编号接口。
    /// </summary>
    public interface IAiukChannelId
    {
        /// <summary>
        /// 以短字符串表示。
        /// </summary>
        /// <returns></returns>
        string AsShortText();

        /// <summary>
        /// 以长字符串表示。
        /// </summary>
        /// <returns></returns>
        string AsLongText();
    }
}
