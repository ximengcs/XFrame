using System;
using XFrame.Modules.Local;

namespace XFrame.Modules.Config
{
    public static class XConfig
    {
        /// <summary>
        /// 默认语言
        /// </summary>
        public static Language Lang;

        /// <summary>
        /// 存档路径
        /// </summary>
        public static string ArchivePath;

        /// <summary>
        /// 资源根路径
        /// </summary>
        public static string ResPath;

        /// <summary>
        /// 本地化语言文件
        /// </summary>
        public static string LocalizeFile;

        /// <summary>
        /// 默认Log辅助器
        /// </summary>
        public static Type DefaultLogger;

        /// <summary>
        /// 默认资源加载辅助器
        /// </summary>
        public static Type DefaultRes;

        /// <summary>
        /// 默认Json序列化处理器
        /// </summary>
        public static Type DefaultJsonSerializer;
    }
}
