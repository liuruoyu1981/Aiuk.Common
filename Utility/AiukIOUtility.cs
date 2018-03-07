using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Aiuk.Common.Utility
{
    /// <summary>
    /// IO工具。
    /// 1. 目录操作。
    /// 2. 文件操作。
    /// 3. 路径操作。
    /// </summary>
    public static class AiukIOUtility
    {
        #region 字段

        private static List<string> m_Dirs;

        /// <summary>
        /// 上次查找的目录列表结果。
        /// </summary>
        private static List<string> Dirs
        {
            get { return m_Dirs ?? (m_Dirs = new List<string>()); }
            set { m_Dirs = value; }
        }

        #endregion

        #region 目录操作

        /// <summary>
        /// 获得指定目录下所有子目录并过滤指定条件目录
        /// </summary>
        /// <param name="dir">目标目录。</param>
        /// <param name="selectFunc">过滤委托。</param>
        /// <returns></returns>
        public static List<string> GetAllDir(string dir, Func<string, bool> selectFunc = null)
        {
            Dirs.Clear();
            GetDirs(dir);
            if (selectFunc != null)
            {
                Dirs = Dirs.Where(selectFunc).ToList();
            }

            return Dirs;
        }

        /// <summary>
        /// 获得指定目录下所有子目录并过滤指定条件目录。
        /// </summary>
        /// <param name="dir">目标目录。</param>
        /// <returns></returns>
        private static void GetDirs(string dir)
        {
            if (string.IsNullOrEmpty(dir))
            {
                AiukDebugUtility.LogError("目标目录为空！");
                return;
            }

            if (!Directory.Exists(dir))
            {
                AiukDebugUtility.LogError(string.Format("目标字符串{0}不是一个有效目录", dir));
                return;
            }

            if (Directory.GetDirectories(dir).Length <= 0) return;

            var sonDirs = Directory.GetDirectories(dir);
            foreach (var sonDir in sonDirs)
            {
                var tempDir = string.Empty;
                if (!sonDir.EndsWith("/"))
                {
                    tempDir = sonDir + "/";
                }

                tempDir = tempDir.Replace("\\", "/");
                Dirs.Add(tempDir);
                GetDirs(tempDir);
            }
        }

        /// <summary>
        /// 确保指定的目录或路径上的所有目录的存在性。
        /// </summary>
        /// <param name="targetPath"></param>
        public static void EnsureDirExist(string targetPath)
        {
            var lastIndex = targetPath.LastIndexOf("/", StringComparison.Ordinal);
            var lastDir = targetPath.Substring(0, lastIndex);
            TryCreateDirectory(lastDir);
        }

        /// <summary>
        /// 如果指定的目录不存在则创建一个新目录。
        /// </summary>
        /// <param name="targetDir">目标目录。</param>
        /// <param name="isHide">是否隐藏刚创建的目录。</param>
        /// <returns></returns>
        public static void TryCreateDirectory(string targetDir, bool isHide = false)
        {
            if (Directory.Exists(targetDir)) return;

            Directory.CreateDirectory(targetDir);
            if (isHide)
            {
                File.SetAttributes(targetDir, FileAttributes.Hidden);
            }
        }

        /// <summary>
        /// 使用系统的默认应用打开指定路径上的目录或者文件。
        /// </summary>
        /// <param name="path"></param>
        public static void OpenFolderOrFile(string path)
        {
            System.Diagnostics.Process.Start(path);
        }

        /// <summary>
        /// 克隆目标目录及其下所有子目录的目录树结构到目标目录。
        /// </summary>
        /// <param name="leftDir">目录一。</param>
        /// <param name="rightDir">目录二。</param>
        private static void CloneDirectoryTree(string leftDir, string rightDir)
        {
            var dirs = GetAllDir(leftDir);
            foreach (var dir in dirs)
            {
                var newDir = dir.Replace(leftDir, rightDir);
                TryCreateDirectory(newDir);
            }
        }

        /// <summary>
        /// 同步两个目录及其子目录下的所有文件。
        /// </summary>
        /// <param name="leftDir">目录一。</param>
        /// <param name="righyDir">目录二。</param>
        /// <param name="filter">文件过滤器委托。</param>
        /// <param name="beforeCopy">拷贝操作前委托。</param>
        public static void SyncDirectory(string leftDir, string righyDir,
            Func<string, bool> filter = null, Func<string, string> beforeCopy = null)
        {
            EnsureDirExist(leftDir);
            EnsureDirExist(righyDir);

            CloneDirectoryTree(leftDir, righyDir);
            var allFiles = filter == null ? GetPaths(leftDir) : GetPaths(leftDir, filter);
            allFiles.ForEach(p => TryCopy(p, p.Replace(leftDir, righyDir), true, beforeCopy));
        }

        /// <summary>
        /// 拷贝一个目录及其所有子目录及文件到目标目录下.
        /// </summary>
        /// <param name="leftDir">待拷贝源目录。</param>
        /// <param name="rightDir">拷贝目标目录。</param>
        /// <param name="isDeleteExist">是否删除已经存在的目录和文件。</param>
        public static void CopyDirectory(string leftDir, string rightDir, bool isDeleteExist = false)
        {
            var files = GetPaths(leftDir);
            CloneDirectoryTree(leftDir, rightDir);
            foreach (var path in files)
            {
                var newPath = path.Replace(leftDir, rightDir);
                TryCopy(path, newPath, isDeleteExist);
            }
        }

        /// <summary>
        /// 删除一个目录及其所有子目录和文件。
        /// </summary>
        /// <param name="dir">目标目录</param>
        public static void DeleteDirectory(string dir)
        {
            if (!Directory.Exists(dir)) return;

            var di = new DirectoryInfo(dir);
            di.Delete(true);
        }

        #endregion

        #region 文件操作

        /// <summary>
        /// 将指定文件移动到目标目录中。
        /// 如果目标目录不存在将会自动创建新目录。
        /// </summary>
        /// <param name="sourceFile">源文件。</param>
        /// <param name="targetDir">目标路径。</param>
        public static void Move(string sourceFile, string targetDir)
        {
            var parentPath = GetParnetPath(targetDir);
            if (!Directory.Exists(parentPath))
            {
                Directory.CreateDirectory(parentPath);
            }

            File.Move(sourceFile, targetDir);
        }

        /// <summary>
        /// 将指定的文件拷贝到目标路径上。
        /// 如果目标路径上已有文件并且isDelete参数为真时则会比较两个文件的Md5值，
        /// 如果不想等则会覆盖目标路径上的文件，如果相等则拷贝操作取消。
        /// </summary>
        /// <param name="sourcePath">源文件路径。</param>
        /// <param name="newPath">目标文件路径。</param>
        /// <param name="isDelete">当目标文件路径上已经存在文件是，是否删除目标文件。</param>
        /// <param name="beforeFunc">拷贝开始前用于修正目标路径的委托。</param>
        public static void TryCopy(string sourcePath, string newPath, bool isDelete = false,
            Func<string, string> beforeFunc = null)
        {
            if (beforeFunc != null)
            {
                newPath = beforeFunc(newPath);
            }

            if (!File.Exists(newPath))
            {
                EnsureDirExist(newPath);
                File.Copy(sourcePath, newPath);
            }

            if (!isDelete) return;
            if (AiukMd5Utility.CompareTwoFileMd5(sourcePath, newPath)) return;

            File.Delete(newPath);
            File.Copy(sourcePath, newPath);
        }

        /// <summary>
        /// 如果指定的路径上有文件存在则删除。
        /// </summary>
        /// <param name="path">目标路径。</param>
        public static void TryDeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        /// <summary>
        /// 删除目录下的所有文件。
        /// </summary>
        /// <param name="dir">目标目录。</param>
        public static void DeleteAllFile(string dir)
        {
            if (!Directory.Exists(dir))
            {
                AiukDebugUtility.LogError(string.Format("给定的字符串{0}不是一个有效目录！", dir));
                return;
            }

            var paths = GetPaths(dir);
            paths.ForEach(TryDeleteFile);
        }

        /// <summary>
        /// 在指定的路径上创建文本文件并写入指定内容。
        /// 该方法会自动创建对应的目录树结构。
        /// </summary>
        /// <param name="path">目标路径。</param>
        /// <param name="content">要写入的文本内容。</param>
        public static void WriteAllText(string path, string content)
        {
            EnsureDirExist(path);
            File.WriteAllText(path, content);
        }

        public static void WriteAllBytes(string path, byte[] bytes)
        {
            EnsureDirExist(path);
            File.WriteAllBytes(path, bytes);
        }

        /// <summary>
        /// 使用指定的字节数组创建一个二进制文件。
        /// </summary>
        /// <param name="path">目标文件路径。</param>
        /// <param name="buffer">字节数组。</param>
        public static void CreateBinaryFile(string path, byte[] buffer)
        {
            EnsureDirExist(path);
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            {
                using (var bw = new BinaryWriter(fs))
                {
                    bw.Write(buffer);
                }
            }
        }

        #endregion

        #region 路径操作

        /// <summary>
        /// 获得指定路径或文件的上级路径或目录。
        /// </summary>
        /// <param name="p">指定路径。</param>
        private static string GetParnetPath(string p)
        {
            var fn = p.Split('/').Last();
            var parentP = p.Replace(fn, "");
            return parentP;
        }

        /// <summary>
        /// 获得指定目录及所有子目录下所有文件路径。
        /// </summary>
        /// <param name="dir">目标目录。</param>
        /// <param name="fileFilter">文件过滤委托。</param>
        /// <param name="dirFilter">目录过滤委托。</param>
        /// <returns></returns>
        public static List<string> GetPaths(string dir, Func<string, bool> fileFilter = null,
            Func<string, bool> dirFilter = null)
        {
            var dirs = GetAllDir(dir, dirFilter);
            var paths = new List<string>();

            foreach (var d in dirs)
            {
                var files = Directory.GetFiles(d).ToList();
                files = files.Select(p => p.Replace("\\", "/")).ToList();
                paths.AddRange(files);
            }

            if (fileFilter != null)
            {
                paths = paths.Where(fileFilter).ToList();
            }

            return paths;
        }

        /// <summary>
        /// 获得指定目录下及其所有子目录中所有文件的文件名及文件路径字典。
        /// </summary>
        /// <param name="dir">指定目录</param>
        /// <param name="fileFilter">文件过滤器。</param>
        /// <param name="dirFilter">目录过滤器。</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetPathDictionary(string dir,
            Func<string, bool> fileFilter = null, Func<string, bool> dirFilter = null)
        {
            var pathDictionary = new Dictionary<string, string>();

            var paths = GetPaths(dir, fileFilter, dirFilter);
            foreach (var path in paths)
            {
                var fileName = Path.GetFileNameWithoutExtension(path);
                if (pathDictionary.ContainsKey(fileName))
                {
                    AiukDebugUtility.LogError(string.Format("目标目录及其子目录下存在同名文件，文件名为：{0}", fileName));
                    continue;
                }

                pathDictionary.Add(fileName, path);
            }

            return pathDictionary;
        }

        #endregion
    }
}