#region Head

// Author:        liuruoyu1981
// CreateDate:    3/7/2018 10:47:08 AM
// Email:         liuruoyu1981@gmail.com || 35490136@qq.com

#endregion

namespace AiukUnityRuntime
{
    /// <summary>
    /// ����ӿڡ�
    /// ʵ���˸ýӿڵ����Ϳ��Է���һ��ȫ�¸�����
    /// ��ȫ�¸������޸Ĳ���Ӱ�쵽ԭ��ʵ����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAiukDeepCopy<out T> where T : class, new()
    {
        /// <summary>
        /// ������ǰʵ��������ȫ�¸�����
        /// </summary>
        /// <returns></returns>
        T DeepCopy();
    }
}
