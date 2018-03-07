#region Head

// Author:        liuruoyu1981
// CreateDate:    3/7/2018 10:47:08 AM
// Email:         liuruoyu1981@gmail.com || 35490136@qq.com

#endregion

namespace AiukUnityRuntime
{
    /// <summary>
    /// 深拷贝接口。
    /// 实现了该接口的类型可以返回一个全新副本。
    /// 对全新副本的修改不会影响到原有实例。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAiukDeepCopy<out T> where T : class, new()
    {
        /// <summary>
        /// 拷贝当前实例并返回全新副本。
        /// </summary>
        /// <returns></returns>
        T DeepCopy();
    }
}
