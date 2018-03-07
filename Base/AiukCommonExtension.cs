using System;
using System.Collections.Generic;
using System.Text;
using Aiuk.Common.Utility;

namespace Aiuk.Common.Base
{
    public static class AiukCommonExtension
    {
        #region StringBuilder

        /// <summary>
        /// 清空一个字符串构建实例
        /// </summary>
        /// <param name="stringBuilder"></param>
        public static void Clear(this StringBuilder stringBuilder)
        {
            stringBuilder.Remove(0, stringBuilder.Length);
        }

        /// <summary>
        /// 追加标准的文件注释头。
        /// 
        /// </summary>
        public static void AppendStandardHeader()
        {
        }

        #endregion

        #region 集合

        #region 字典

        /// <summary>
        /// 合并两个字典
        /// </summary>
        /// <typeparam name="TKey">字典键泛型类型。</typeparam>
        /// <typeparam name="TValue">字典值泛型类型。</typeparam>
        /// <param name="left">字典一。</param>
        /// <param name="right">字典二。</param>
        /// <returns></returns>
        public static void Combin<TKey, TValue>(this IDictionary<TKey, TValue> left, Dictionary<TKey, TValue> right)
        {
            foreach (var keyValuePair in right)
            {
                if (left.ContainsKey(keyValuePair.Key))
                {
                    AiukDebugUtility.LogError(string.Format("Key{0}当前已存在，请检查！", keyValuePair.Key));
                }
                else
                {
                    left.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }
        }

        #endregion

        #endregion

        #region 类型转换关键字函数

        /// <summary>
        /// 尝试将一个对象转型为指定类型。
        /// 成功返回真，失败返回假。
        /// </summary>
        /// <typeparam name="T">要转换的目标类型。</typeparam>
        /// <param name="t">实例对象。</param>
        /// <returns></returns>
        public static bool As<T>(this object t) where T : class
        {
            var instance = t as T;
            var result = instance != null;
            return result;
        }

        #endregion

        #region 基础语法扩展

        public static void IfElse(bool condition, Action trueAction, Action falseAction)
        {
            if (condition)
            {
                trueAction();
            }
            else
            {
                falseAction();
            }
        }

        public static void IfElse<T>(bool condition, T data, Action<T> trueAction, Action<T> falseAction)
        {
            if (condition)
            {
                trueAction(data);
            }
            else
            {
                falseAction(data);
            }
        }

        public static void IfElse<T1, T2>(bool condition, T1 data1,
            T2 data2, Action<T1> trueAction, Action<T2> falseAction)
        {
            if (condition)
            {
                trueAction(data1);
            }
            else
            {
                falseAction(data2);
            }
        }

        #endregion

    }
}