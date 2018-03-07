using System;
using System.Diagnostics;
using System.IO;

namespace Aiuk.Common.Utility
{
    public static class AiukCmdUtility
    {
               /// <summary>
        /// 执行Dos命令列表
        /// </summary>
        /// <param name="cmdArray"></param>
        public static void ExcuteDosCommand(params string[] cmdArray)
        {
            try
            {
                var p = new Process
                {
                    StartInfo =
                    {
                        FileName = "cmd",
                        UseShellExecute = false,
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = false
                    }
                };

                p.OutputDataReceived += SortProcess_OutputDataReceived;
                p.Start();
                var cmdWriter = p.StandardInput;
                p.BeginOutputReadLine();
                foreach (var cmd in cmdArray)
                {
                    cmdWriter.WriteLine(cmd);
                }
                cmdWriter.Close();
                p.WaitForExit();
                p.Close();
            }
            catch (Exception ex)
            {
                AiukDebugUtility.LogError("执行命令失败，请检查输入的命令是否正确！异常信息为：" + ex.Message);
            }
        }

        /// <summary>
        /// 用于接收windows命令行传回的日志消息。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void SortProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                AiukDebugUtility.Log(e.Data);
            }
        }

        /// <summary>  
        /// 注册表导入。
        /// </summary>  
        /// <param name="regPath">注册表文件路径</param>  
        public static void RegistryInject(string regPath)
        {
            if (!File.Exists(regPath)) return;

            regPath = @"""" + regPath + @"""";
            Process.Start("regedit", string.Format("/s {0}", regPath));
        } 
    }
}