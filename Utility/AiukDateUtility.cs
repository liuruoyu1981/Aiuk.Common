using System;

namespace Aiuk.Common
{
    public static class AiukDateUtility
    {
        /// <summary>
        /// 获得当前时间的完整年月日时分秒表示字符串。
        /// </summary>
        /// <value>The date and time.</value>
        public static string DateAndTimeForNow
        {
            get
            {
                var value = DateTime.Now.ToString("G");
                return value;
            }
        }

        /// <summary>
        /// 获得当前时间的数字Id表示，表示为一个Int型数字。
        /// </summary>
        /// <value>The now number identifier.</value>
        public static string NowNumId
        {
            get
            {
                var value = DateTime.Now.ToString("MMDDHHmmss");
                return value;
            }
        }


    }

}

