using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

namespace Aiuk.Common.Utility
{
    /// <summary>
    /// 序列化工具。
    /// </summary>
    public static class AiukSerializeUtility
    {
        /// <summary>
        /// C#二进制序列化。
        /// </summary>
        /// <returns>The serialize.</returns>
        /// <param name="value">Value.</param>
        public static byte[] Serialize(object value)
        {
            if (value == null) return null;

            using (var ms = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                bf.Serialize(ms, value);
                var bytes = new byte[ms.Length];
                Buffer.BlockCopy(ms.GetBuffer(), 0, bytes, 0, (int)ms.Length);
                return bytes;
            }
        }

        /// <summary>
        /// C#二进制反序列化。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T DeSerialize<T>(byte[] value) where T : class, new()
        {
            if (value == null) return default(T);

            using (var ms = new MemoryStream(value))
            {
                var bf = new BinaryFormatter();
                var instance = (T)bf.Deserialize(ms);
                return instance;
            }
        }



    }
}


