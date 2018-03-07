using System.IO;
using System.Xml.Serialization;

namespace Aiuk.Common.Utility
{
    /// <summary>
    /// Xml工具。
    /// 1. 序列化。
    /// 2. 反序列化。
    /// </summary>
    public static class AiukXmlUtility
    {
        /// <summary>
        /// 将指定实例序列化为xml文件并保存在指定路径。
        /// </summary>
        /// <typeparam name="T">泛型类型。</typeparam>
        /// <param name="filePath">目标保存路径。</param>
        /// <param name="sourceObj">源实例。</param>
        /// <param name="xmlRootName"></param>
        public static void SaveToXml<T>(string filePath, T sourceObj, string xmlRootName = null)
            where T : class
        {
            if (string.IsNullOrEmpty(filePath) || sourceObj == null) return;

            var type = typeof(T);
            xmlRootName = xmlRootName ?? typeof(T).Name;

            using (var writer = new StreamWriter(filePath))
            {
                var xmlSerializer = string.IsNullOrEmpty(xmlRootName) ?
                    new XmlSerializer(type) :
                    new XmlSerializer(type, new XmlRootAttribute(xmlRootName));
                xmlSerializer.Serialize(writer, sourceObj);
            }
        }

        /// <summary>
        /// 加载一个目标Xml文件并反序列化为指定类型实例。
        /// </summary>
        /// <typeparam name="T">泛型类型。</typeparam>
        /// <param name="filePath">目标文件。</param>
        /// <returns></returns>
        public static T LoadFromXml<T>(string filePath) where T : class
        {
            var type = typeof(T);
            T result;

            if (!File.Exists(filePath)) return default(T);

            using (var reader = new StreamReader(filePath))
            {
                var xmlSerializer = new XmlSerializer(type);
                result = xmlSerializer.Deserialize(reader) as T;
            }

            return result;
        }



    }
}