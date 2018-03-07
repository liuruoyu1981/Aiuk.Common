namespace Aiuk.Common.PoolCache
{
    /// <summary>
    /// 可回收接口。
    /// </summary>
    public interface IAiukRecycle
    {
        /// <summary>
        /// 归还实例以便下次重新使用。
        /// </summary>
        void Restore();

        /// <summary>
        /// 重置自身状态以便重新使用。
        /// </summary>
        void Reset();

    }
}