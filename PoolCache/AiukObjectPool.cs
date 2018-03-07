using System;

namespace Aiuk.Common.PoolCache
{
    /// <summary>
    /// 基础泛型对象池。
    /// </summary>
    public class AiukObjectPool<T> : IAiukObjectPool<T> where T : class
    {
        #region Instance Field

        /// <summary>
        /// 缓存对象数组
        /// </summary>
        private T[] m_ObjectArray;

        /// <summary>
        /// 对象池关键字
        /// </summary>
        private readonly string m_PoolKey;

        #endregion

        #region Instance Method

        /// <summary>
        /// 基础泛型对象池
        /// </summary>
        /// <param name="factoryFunc"></param>
        /// <param name="count"></param>
        /// <param name="poolKey"></param>
        /// <param name="onCreated"></param>
        public AiukObjectPool(Func<T> factoryFunc, int count,
            string poolKey = null, Action<T> onCreated = null)
        {
            m_ObjectArray = new T[count];
            //  填充对象池
            for (var i = 0; i < m_ObjectArray.Length; i++)
            {
                var obj = factoryFunc();
                if (onCreated != null)
                {
                    onCreated(obj);
                }
                m_ObjectArray[i] = obj;
            }

            m_PoolKey = poolKey;
        }

        /// <summary>
        /// 已使用对象数量
        /// </summary>
        public int UseCount { get; private set; }

        /// <summary>
        /// 获取一个对象
        /// 如果对象池中有空闲对象，则默认返回第一个
        /// 如果对象池中没有空闲对象，则使用工厂创建一个
        /// </summary>
        /// <returns>返回一个实例</returns>
        public T Take()
        {
            while (true)
            {
                //  耗尽
                if (UseCount == m_ObjectArray.Length - 1)
                {
                    //  增长对象数组
                    var length = m_ObjectArray.Length;
                    length += Convert.ToInt32(length * 0.5);
                    var oldArray = m_ObjectArray;
                    m_ObjectArray = new T[length];
                    for (var i = 0; i < oldArray.Length; i++)
                    {
                        m_ObjectArray[i] = oldArray[i];
                    }

                    continue;
                }

                var obj = m_ObjectArray[UseCount];
                UseCount++;
                return obj;
            }
        }

        /// <summary>
        /// 归还一个对象到对象池
        /// 如果对象池已达上限，则丢弃该对象
        /// 如果对象池没有到达上限，则回收该对象
        /// </summary>
        /// <param name="t">归还的实例</param>
        public void Restore(T t)
        {
            UseCount--;
            m_ObjectArray[UseCount] = t;
        }

        /// <summary>
        /// 清空对象池
        /// </summary>
        public void Clear()
        {
            m_ObjectArray = null;
        }

        /// <summary>
        /// 对对象池中的每个缓存对象执行指定操作
        /// </summary>
        /// <param name="del"></param>
        public void ForEach(Action<T> del)
        {
            foreach (var x1 in m_ObjectArray)
            {
                if (x1 != null)
                {
                    del(x1);
                }
            }
            m_ObjectArray = null;
        }

        #endregion

        #region Static Method



        #endregion

    }
}