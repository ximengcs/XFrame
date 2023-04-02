using XFrame.Modules.Local;

namespace XFrame.Modules.Config
{
    /// <summary>
    /// 模块配置
    /// </summary>
    public static class XConfig
    {
        /// <summary>
        /// 入口
        /// </summary>
        public static string Entrance;

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
        public static string DefaultLogger;

        /// <summary>
        /// 默认资源加载辅助器
        /// </summary>
        public static string DefaultRes;

        /// <summary>
        /// 默认Json序列化处理器
        /// </summary>
        public static string DefaultJsonSerializer;

        /// <summary>
        /// 默认下载辅助器
        /// </summary>
        public static string DefaultDownloadHelper;
    }
}
