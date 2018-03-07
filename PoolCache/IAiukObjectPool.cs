namespace Aiuk.Common.PoolCache
{
    /// <summary>
    /// 泛型对象池接口。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAiukObjectPool<T> where T : class
    {
        T Take();

        /// <summary>
        /// 归还一个对象。
        /// </summary>
        /// <returns>The restore.</returns>
        /// <param name="t">T.</param>
        void Restore(T t);

        int UseCount { get; }

        void Clear();
    }
}