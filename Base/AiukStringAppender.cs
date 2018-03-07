using System;
using System.Collections.Generic;
using System.Text;
using Aiuk.Common.PoolCache;
using Aiuk.Common.Utility;

namespace Aiuk.Common.Base
{
    /// <summary>
    /// 字符串追加器。
    /// 提供常用的字符串动态构建方法封装。
    /// 提供各种语言脚本创建的API封装。
    /// </summary>
    public class AiukStringAppender : IDisposable
    {
        private readonly StringBuilder m_Builder;

        /// <summary>
        /// 缩进宽度。
        /// </summary>
        private int m_SpaceWidth;

        private void AppendSpace()
        {
            for (int i = 0; i < m_SpaceWidth; i++)
            {
                m_Builder.Append(" ");
            }
        }

        public void ToLeft()
        {
            var result = m_SpaceWidth - 4;
            if (result < 0)
            {
                AiukDebugUtility.Log("缩进不能为负数！");
                return;
            }

            m_SpaceWidth = result;
        }

        public void ToRight()
        {
            m_SpaceWidth += 4;
        }

        public string Content
        {
            get { return m_Builder.ToString(); }
        }

        public AiukStringAppender()
        {
            m_Builder = AiukCommonFactory.StringBuilderPool.Take();
        }

        public void Dispose()
        {
            m_Builder.Clear();
            AiukCommonFactory.StringBuilderPool.Restore(m_Builder);
        }

        public override string ToString()
        {
            return m_Builder.ToString();
        }

        public void Clean()
        {
            m_Builder.Clear();
        }

        #region Append方法


        public static AiukStringAppender StartAppend(string format, params object[] args)
        {
            var builder = new AiukStringAppender();
            builder.m_Builder.AppendFormat(format, args);
            return builder;
        }

        public static AiukStringAppender StartAppend(string format)
        {
            var builder = new AiukStringAppender();
            builder.m_Builder.Append(format);
            return builder;
        }

        public static AiukStringAppender StartAppendLine(string value)
        {
            var builder = new AiukStringAppender();
            builder.m_Builder.AppendLine(value);
            return builder;
        }

        public static AiukStringAppender StartAppendLine(string format, params object[] args)
        {
            var builder = new AiukStringAppender();
            builder.m_Builder.AppendFormat(format, args);
            builder.m_Builder.AppendLine();
            return builder;
        }

        public AiukStringAppender Append(string format, params object[] args)
        {
            m_Builder.AppendFormat(format, args);
            return this;
        }

        public AiukStringAppender Append(string format)
        {
            m_Builder.Append(format);
            return this;
        }

        public AiukStringAppender AppendLine()
        {
            AppendSpace();
            m_Builder.AppendLine();
            return this;
        }

        public AiukStringAppender AppendLine(string value)
        {
            AppendSpace();
            m_Builder.AppendLine(value);
            return this;
        }

        public AiukStringAppender AppendLine(string value, params object[] args)
        {
            AppendSpace();
            m_Builder.AppendFormat(value, args);
            m_Builder.AppendLine();
            return this;
        }

        #endregion

        #region Cs

        /// <summary>
        /// 追加Cs注释。
        /// </summary>
        /// <param name="bodyComment">主体注释文本。</param>
        /// <param name="paramNames">参数名注释列表。</param>
        /// <param name="paramComments">参数注释列表。</param>
        public void AppendCsComment
        (
            string bodyComment,
            List<string> paramNames = null,
            List<string> paramComments = null
        )
        {
            AppendLine("/// <summary>");
            AppendLine("/// " + bodyComment);
            AppendLine("/// </summary>");

            if (paramNames == null) return;

            if (paramComments == null)
            {
                foreach (var paramName in paramNames)
                {
                    AppendLine("/// <param name=\"" + paramName + "\"></param>");
                }
            }
            else
            {
                for (var i = 0; i < paramNames.Count; i++)
                {
                    var name = paramNames[i];
                    var comment = paramComments[i];
                    AppendLine("/// <param name=\"{0}\">{1}</param>", name, comment);
                }
            }
        }

        #endregion
    }
}