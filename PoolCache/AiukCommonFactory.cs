using System.Text;

namespace Aiuk.Common.PoolCache
{
    /// <summary>
    /// 静态通用工厂。
    /// 1. 持有各种常用的对象池。
    /// </summary>
    public static class AiukCommonFactory
    {
        private static AiukObjectPool<StringBuilder> _StringBuilderPool;

        /// <summary>
        /// 字符串构造器对象池，默认容量为50。
        /// </summary>
        public static AiukObjectPool<StringBuilder> StringBuilderPool
        {
            get
            {
                if (_StringBuilderPool != null) return _StringBuilderPool;

                _StringBuilderPool = new AiukObjectPool<StringBuilder>(() => new StringBuilder(), 50);
                return _StringBuilderPool;
            }
        }


    }
}