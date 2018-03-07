using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Aiuk.Common.Base;

namespace Aiuk.Common.Utility
{
    /// <summary>
    /// 反射工具。
    /// 1. 程序集缓存。
    /// </summary>
    public static class AiukReflectUtility
    {
        /// <summary>
        /// 程序集公开类型缓存字典
        /// 将当前已经查找过的程序集的公开类型列表缓存在该字典中
        /// 后续查找直接使用缓存列表提高反射效率
        /// </summary>
        private static readonly Dictionary<string, List<Type>>
            AssemblyDictionary = new Dictionary<string, List<Type>>();

        /// <summary>
        /// 尝试获取已缓存的程序集的公开类型列表。
        /// 如果目标程序集当前没有缓存则会返回空。
        /// </summary>
        /// <param name="assemblyName">目标程序集的全名。</param>
        /// <returns></returns>
        private static List<Type> TryGetCachedAssemblyTypes(string assemblyName)
        {
            if (!AssemblyDictionary.ContainsKey(assemblyName)) return null;

            var result = AssemblyDictionary[assemblyName];
            return result;
        }

        /// <summary>
        /// 获得目标程序及的类型列表。
        /// </summary>
        /// <param name="assembly">目标程序集。</param>
        /// <returns></returns>
        public static List<Type> GetAllType(Assembly assembly)
        {
            var types = TryGetCachedAssemblyTypes(assembly.FullName);
            if (types != null)
            {
                return types;
            }

            types = assembly.GetTypes().ToList();
            AssemblyDictionary.Add(assembly.FullName, types);
            return types;
        }

        /// <summary>
        /// 在目标程序集中查找实现了指定类型的所有子类并返回以类型名为Key，类型为值的字典。
        /// 目标类型不能是接口或者抽象类。
        /// </summary>
        /// <typeparam name="T">目标类型。</typeparam>
        /// <param name="assembly">目标程序集，默认为当前执行的程序集。</param>
        /// <param name="whereFunc">类型过滤委托，默认为空。</param>
        /// <returns></returns>
        private static Dictionary<string, Type> GetTypeDictionary<T>(
            Assembly assembly = null, Func<Type, bool> whereFunc = null)
        {
            var targetAssembly = assembly ?? Assembly.GetExecutingAssembly();
            var asmTypes = GetAllType(targetAssembly);

            var result = asmTypes
                .Where(t => typeof(T).IsAssignableFrom(t))
                .Where(t => !t.IsInterface && !t.IsAbstract);
            if (whereFunc == null)
            {
                return result.ToDictionary(t => t.Name);
            }

            var whereResult = result.Where(whereFunc).ToDictionary(t => t.Name);
            return whereResult;
        }

        /// <summary>
        /// 在目标程序集列表中查找指定类型的所有子类并返回以类型名为Key，类型为值的字典。
        /// 目标类型不能是接口或者抽象类。
        /// </summary>
        /// <typeparam name="T">目标类型。</typeparam>
        /// <param name="assemblies">目标程序集列表，默认为当前执行的程序集。</param>
        /// <param name="whereFunc">类型过滤委托，默认为空。</param>
        /// <returns></returns>
        public static Dictionary<string, Type> GetTypeDictionary<T>(
            List<Assembly> assemblies = null, Func<Type, bool> whereFunc = null)
        {
            assemblies = assemblies ?? new List<Assembly> {Assembly.GetExecutingAssembly()};
            var typeDictionary = new Dictionary<string, Type>();

            foreach (var assembly in assemblies)
            {
                var rightDictionary = GetTypeDictionary<T>(assembly, whereFunc);
                typeDictionary.Combin(rightDictionary);
            }

            return typeDictionary;
        }

        /// <summary>
        /// 在目标程序集中查找指定类型的所有子类型并返回类型列表。
        /// </summary>
        /// <typeparam name="T">目标类型。</typeparam>
        /// <param name="assembly">目标程序集。</param>
        /// <param name="isInterface">是否要过滤接口类型。</param>
        /// <param name="isAbstract">是否要过滤抽象类型。</param>
        /// <returns></returns>
        private static List<Type> GetTypeList<T>(Assembly assembly,
            bool isInterface = false, bool isAbstract = false)
        {
            var targetAssembly = assembly ?? Assembly.GetExecutingAssembly();
            var asmTypes = GetAllType(targetAssembly);

            var result = asmTypes
                .Where(t => typeof(T).IsAssignableFrom(t)
                            && t.IsInterface == isInterface
                            && t.IsAbstract == isAbstract)
                .ToList();

            return result;
        }

        /// <summary>
        /// 在目标程序集列表中查找指定类型的所有子类型并返回类型列表。
        /// </summary>
        /// <typeparam name="T">目标类型。</typeparam>
        /// <param name="assemblies">目标程序集。</param>
        /// <param name="isInterface">是否要过滤接口类型。</param>
        /// <param name="isAbstract">是否要过滤抽象类型。</param>
        /// <returns></returns>
        public static List<Type> GetTypeList<T>(bool isInterface = false, bool isAbstract = false,
            params Assembly[] assemblies)
        {
            var asemList = assemblies != null
                ? assemblies.ToList()
                : new List<Assembly> {Assembly.GetExecutingAssembly()};
            var asmTypes = new List<Type>();

            foreach (var assembly in asemList)
            {
                var types = GetTypeList<T>(assembly);
                asmTypes.AddRange(types);
            }

            return asmTypes;
        }

        /// <summary>
        /// 使用反射调用目标类型的静态函数。
        /// </summary>
        /// <param name="type">目标类型。</param>
        /// <param name="methodName">函数名。</param>
        /// <param name="parames">函数所需的调用参数数组。</param>
        public static void InvokeStaticMethod(Type type, string methodName, object[] parames)
        {
            var methodInfo = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic);
            if (methodInfo == null)
            {
                AiukDebugUtility.LogError(string.Format("无法从目标类型{0}中找到指定的静态函数{1}。", type, methodName));
                return;
            }

            try
            {
                methodInfo.Invoke(null, parames);
            }
            catch (Exception exception)
            {
                AiukDebugUtility.LogError(string.Format("调用类型{0}的静态函数{1}时发生异常，异常信息为{2}",
                    type, methodName, exception.Message));
                AiukDebugUtility.LogError(exception.InnerException.Message);
            }
        }

        /// <summary>
        /// 获得指定枚举类型的所有字段信息。
        /// </summary>
        /// <param name="type">目标枚举类型。</param>
        /// <returns></returns>
        public static List<FieldInfo> GetEnumAllFieldInfos(Type type)
        {
            if (type == null)
            {
                AiukDebugUtility.LogError("目标类型为空！");
                return null;
            }

            if (!type.IsEnum)
            {
                AiukDebugUtility.LogError(string.Format("目标类型{0}不是一个枚举类型！", type));
                return null;
            }

            var fieldInfos = type.GetFields().ToList();
            fieldInfos.RemoveAt(0);
            return fieldInfos;
        }
    }
}