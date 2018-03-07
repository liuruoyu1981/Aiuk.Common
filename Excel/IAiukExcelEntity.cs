#region Head

// Author:        liuruoyu1981
// CreateDate:    3/7/2018 10:42:05 AM
// Email:         liuruoyu1981@gmail.com || 35490136@qq.com

#endregion

using System.Collections.Generic;
using AiukUnityRuntime;

namespace Aiuk.Common
{
    /// <summary>
    /// Excel数据实体。
    /// 每个实例对应Excel表中的一条数据。
    /// 每个实例都可以返回一个全新副本,对副本的修改不会影响到自身。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAiukExcelEntity<T> : IAiukDeepCopy<T>
        where T : class, new()
    {
        /// <summary>
        /// 生成一个Excel数据实体对象。
        /// </summary>
        /// <param name="row">Exel文本数据源中的一条数据，如txt文本中的一行</param>
        /// <returns></returns>
        T CreateEntity(List<string> row);

        /// <summary>
        /// 使用Excel表的所有行文本数据创建对应的Excel数据实体列表。
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        List<T> CreateEntitys(List<string> rows);
    }
}
