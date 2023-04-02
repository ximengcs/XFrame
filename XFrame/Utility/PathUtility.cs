using System;
using System.IO;
using XFrame.Modules.Diagnotics;

namespace XFrame.Utility
{
    /// <summary>
    /// 路径常用方法
    /// </summary>
    public class PathUtility
    {
        public static int CheckFileName(string fullPath, out string thisName, out string suplusName)
        {
            fullPath = fullPath.Replace("/", "\\");
            fullPath = fullPath.Replace("_", "\\");
            string[] paths = fullPath.Split('\\');
            if (paths.Length == 0)
            {
                Log.Error("XFrame", "DataArchive file tree error");
                thisName = default;
                suplusName = default;
                return default;
            }
            else if (paths.Length == 1)
            {
                thisName = paths[0];
                suplusName = default;
                return 1;
            }
            else
            {
                thisName = paths[0];
                string[] supluses = new string[paths.Length - 1];
                Array.Copy(paths, 1, supluses, 0, paths.Length - 1);
                suplusName = Path.Combine(supluses);
                return 2;
            }
        }
    }
}
