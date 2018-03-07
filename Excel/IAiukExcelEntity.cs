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
    /// Excel����ʵ�塣
    /// ÿ��ʵ����ӦExcel���е�һ�����ݡ�
    /// ÿ��ʵ�������Է���һ��ȫ�¸���,�Ը������޸Ĳ���Ӱ�쵽����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAiukExcelEntity<T> : IAiukDeepCopy<T>
        where T : class, new()
    {
        /// <summary>
        /// ����һ��Excel����ʵ�����
        /// </summary>
        /// <param name="row">Exel�ı�����Դ�е�һ�����ݣ���txt�ı��е�һ��</param>
        /// <returns></returns>
        T CreateEntity(List<string> row);

        /// <summary>
        /// ʹ��Excel����������ı����ݴ�����Ӧ��Excel����ʵ���б�
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        List<T> CreateEntitys(List<string> rows);
    }
}
