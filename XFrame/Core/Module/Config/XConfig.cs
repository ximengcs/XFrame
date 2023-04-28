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
        /// 存档路径
        /// </summary>
        public static string ArchivePath;

        /// <summary>
        /// 存档是否加密
        /// </summary>
        public static bool ArchiveEncrypt;

        /// <summary>
        /// 资源根路径
        /// </summary>
        public static string ResPath;

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

        /// <summary>
        /// 默认数据表辅助器
        /// </summary>
        public static string DefaultDataTableHelper;

        /// <summary>
        /// 默认加密辅助器
        /// </summary>
        public static string DefaultCryptor;

        /// <summary>
        /// 默认Plot辅助器
        /// </summary>
        public static string DefaultPlotHelper;
    }
}
