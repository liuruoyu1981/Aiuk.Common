using System;

#if UNITY_5_3_OR_NEWER

using Aiuk.Common.Base;
using UnityEngine;

#endif

namespace Aiuk.Common.Utility
{
    public static class AiukDebugUtility
    {
#if NETCOREAPP2_0

        public static void Log(string message)
        {
            Console.WriteLine(message);
        }

        public static void LogError(string message)
        {
            Console.WriteLine(message);
        }

#endif


        #region 日志输出委托

        private static readonly Action<string> LogHander;
        private static readonly Action<string> WarningHander;
        private static readonly Action<string> ErrorHander;
        private static readonly Action<Exception> ExceptionHander;

        #endregion

#if UNITY_5_3_OR_NEWER
    static AiukDebugUtility()
    {
        LogHander = Debug.Log;
        WarningHander = Debug.LogWarning;
        ErrorHander = Debug.LogError;
        ExceptionHander = Debug.LogException;
    }

    public static void Log(string message, params object[] args)
    {
        var appender = AiukStringAppender.StartAppendLine(message, args);
        LogHander(appender.Content);
        appender.Dispose();
    }

    public static void Log(string message)
    {
        var appender = AiukStringAppender.StartAppendLine(message);
        LogHander(appender.Content);
        appender.Dispose();
    }

    public static void LogWarning(string message)
    {
        var appender = AiukStringAppender.StartAppendLine(message);
        WarningHander(appender.Content);
        appender.Dispose();
    }

    public static void LogWarning(string message, params object[] args)
    {
        var appender = AiukStringAppender.StartAppendLine(message, args);
        WarningHander(appender.Content);
        appender.Dispose();
    }

    public static void LogError(string message)
    {
        var appender = AiukStringAppender.StartAppendLine(message);
        ErrorHander(appender.Content);
        appender.Dispose();
    }

    public static void LogError(string message, params object[] args)
    {
        var appender = AiukStringAppender.StartAppendLine(message, args);
        ErrorHander(appender.Content);
        appender.Dispose();
    }

    public static void LogException(Exception exception)
    {
        ExceptionHander(exception);
    }

#endif
    }
}